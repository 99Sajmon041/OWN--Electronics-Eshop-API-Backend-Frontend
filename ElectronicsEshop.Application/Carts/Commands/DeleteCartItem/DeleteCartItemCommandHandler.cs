using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Carts.Commands.DeleteCartItem;

public sealed class DeleteCartItemCommandHandler
    (ILogger<DeleteCartItemCommandHandler> logger,
    ICartItemRepository cartItemRepository,
    ICartRepository cartRepository,
    IUserContext userContext,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCartItemCommand>
{
    public async Task Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = userContext.GetCurrentUser();
        if (user is null)
        {
            logger.LogWarning("Není možné smazat položku z košíku, uživatel nebyl nalezen.");
            throw new NotFoundException(nameof(ApplicationUser), "ID");
        }

        var cart = await cartRepository.GetCartForCurrentUserAsync(user.Id, cancellationToken);
        if (cart is null)
        {
            logger.LogWarning("Není možné smazat položku z košíku, košík uživatele s ID: {UserId} nebyl nalezen.", user.Id);
            throw new NotFoundException(nameof(Cart), $"uživatelským ID: {user.Id}");
        }

        var cartItem = await cartItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (cartItem is null || cartItem.CartId != cart.Id)
        {
            logger.LogWarning("Položka nemůže být odstraněna, protože neexistuje, nebo nepatří uživateli.");
            return;
        }

        await productRepository.AddStockQtyAsync(cartItem.Product, cartItem.Quantity, cancellationToken);
        await cartItemRepository.DeleteForCurrentUserAsync(cartItem, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Z košíku byl odstraněn produkt s názvem: {ProductName}.", cartItem.Product.Name);
    }
}
