using ElectronicsEshop.Application.Abstractions;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.Enums;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Carts.Commands.SubmitCart;

public sealed class SubmitCartCommandHandler(
    ILogger<SubmitCartCommandHandler> logger,
    IUserContext userContext,
    ICartRepository cartRepository,
    IOrderRepository orderRepository,
    IPaymentService paymentService,
    ICartItemRepository cartItemRepository,
    UserManager<ApplicationUser> userManager,
    IEmailService emailService,
    IUnitOfWork unitOfWork) : IRequestHandler<SubmitCartCommand>
{
    public async Task Handle(SubmitCartCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = userContext.GetCurrentUser();
        if (user is null)
        {
            logger.LogWarning("Není možné vytvořit objednávku, uživatel nebyl nalezen (IUserContext je null).");
            throw new NotFoundException(nameof(ApplicationUser), "CurrentUser");
        }

        var appUser = await userManager.FindByEmailAsync(user.Email);
        if (appUser is null)
        {
            logger.LogWarning("Není možné vytvořit objednávku, uživatel s e-mailem {UserEmail} nebyl nalezen.", user.Email);
            throw new NotFoundException(nameof(ApplicationUser), user.Email);
        }

        var cart = await cartRepository.GetCartForCurrentUserAsync(user.Id, cancellationToken);
        if (cart is null)
        {
            logger.LogWarning("Není možné vytvořit objednávku, košík uživatele s ID {UserId} nebyl nalezen.", user.Id);
            throw new NotFoundException(nameof(Cart), $"uživatelským ID: {user.Id}");
        }

        if (cart.CartItems.Count == 0)
        {
            logger.LogWarning("Není možné vytvořit objednávku, košík uživatele s e-mailem {UserEmail} je prázdný.", user.Email);
            throw new DomainException("Nelze vytvořit objednávku, košík je prázdný.");
        }

        var totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.UnitPrice);

        var paymentResult = await paymentService.CreatePayment(user.Id, totalAmount, cancellationToken);
        if (!paymentResult.Success)
        {
            logger.LogError("Nelze provést platbu pro uživatele {UserId}. Chyba: {Error}", user.Id, paymentResult.Error);
            throw new DomainException($"Nelze provést platbu: {paymentResult.Error}");
        }

        var order = new Order
        {
            ApplicationUser = appUser,
            ApplicationUserId = appUser.Id,
            TotalAmount = totalAmount,
            CreatedAt = DateTime.UtcNow,
            OrderStatus = OrderStatus.Paid,
            OrderItems = []
        };

        var orderItems = cart.CartItems.Select(x => new OrderItem
        {
            Product = x.Product,
            ProductId = x.ProductId,
            Order = order,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice
        }).ToList();

        order.OrderItems = orderItems;

        await orderRepository.CreateAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await paymentService.AssignOrderAsync(paymentResult.Payment!.Id, order.Id, cancellationToken);

        await cartItemRepository.DeleteAllForCurrentUserAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        try
        {
            await emailService.SendOrderConfirmationEmailAsync(appUser, order, cancellationToken);

            logger.LogInformation("E-mail s potvrzením objednávky {OrderId} byl odeslán na {Email}.", order.Id,  appUser.Email);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Nepodařilo se odeslat e-mail s potvrzením objednávky {OrderId} na {Email}. Objednávka zůstává vytvořená.", order.Id, appUser.Email);
        }

        logger.LogInformation(
            "Objednávka {OrderId} byla úspěšně vytvořena a zaplacena. Uživatelské ID: {UserId}, PaymentId: {PaymentId}",
            order.Id,
            user.Id,
            paymentResult.Payment!.Id);
    }
}
