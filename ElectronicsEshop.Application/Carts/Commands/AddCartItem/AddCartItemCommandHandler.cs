using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Carts.Commands.AddCartItem;

public sealed class AddCartItemCommandHandler
    (ILogger<AddCartItemCommandHandler> logger,
    ICartRepository cartRepository,
    ICartItemRepository cartItemRepository,
    IProductRepository productRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork) : IRequestHandler<AddCartItemCommand>
{
    public async Task Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = userContext.GetCurrentUser();
        if (user is null)
        {
            logger.LogWarning("Není možné přidat položku do košíku, uživatel nebyl nalezen.");
            throw new NotFoundException(nameof(ApplicationUser), "ID");
        }

        var cart = await cartRepository.GetCartForCurrentUserAsync(user.Id, cancellationToken);
        if (cart is null)
        {
            logger.LogWarning("Není možné přidat položku do košíku, košík uživatele s ID: {UserId} nebyl nalezen.", user.Id);
            throw new NotFoundException(nameof(Cart), $"uživatelským ID: {user.Id}");
        }

        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            logger.LogWarning("Není možné přidat položku do košíku, produkt s ID: {ProductId} nebyl nalezen.", request.ProductId);
            throw new NotFoundException(nameof(Product), request.ProductId);
        }

        var stockQty = product.StockQty;
        if (stockQty < request.Quantity)
        {
            logger.LogWarning(
                "Nelze přidat do košíku {Quantity} kusů zboží: {ProductName}, skladem je pouze {StockQuantity} položek.", request.Quantity, product.Name, stockQty);

            throw new ForbiddenException(
                $"Nelze přidat {request.Quantity} kusů do košíku – požadované množství není skladem.");
        }

        var existingCartItem = await cartItemRepository.GetForUserAndProductAsync(
            user.Id,
            product.Id,
            cancellationToken);

        if (existingCartItem is null)
        {
            var cartItem = new CartItem
            {
                ProductId = product.Id,
                CartId = cart.Id,
                Quantity = request.Quantity,
                UnitPrice = product.Price,
                CreatedAt = DateTime.UtcNow
            };

            await cartItemRepository.AddAsync(cartItem, cancellationToken);
        }
        else
        {
            await cartItemRepository.UpdateQuantityAsync(existingCartItem, request.Quantity, cancellationToken);
        }

        await productRepository.AddStockQtyAsync(product, request.Quantity * -1, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Do košíku uživatele s e-mailem {UserEmail} bylo přidáno {Quantity} kusů položky: {ProductName}.", 
            user.Email,
            request.Quantity, 
            product.Name);
    }
}
