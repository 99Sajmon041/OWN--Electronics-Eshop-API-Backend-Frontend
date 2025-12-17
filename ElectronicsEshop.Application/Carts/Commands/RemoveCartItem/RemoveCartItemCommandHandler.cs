using ElectronicsEshop.Application.Carts.Commands.AddCartItem;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Carts.Commands.RemoveCartItem;

public sealed class RemoveCartItemCommandHandler(
    ILogger<RemoveCartItemCommandHandler> logger,
    ICartRepository cartRepository,
    ICartItemRepository cartItemRepository,
    IProductRepository productRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveQtyCartItemComman>
{
    public async Task Handle(RemoveQtyCartItemComman request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = userContext.GetCurrentUser();
        if (user is null)
        {
            logger.LogWarning("Není možné odebrat položku z košíku – uživatel není autentizovaný.");
            throw new UnauthorizedException("Uživatel není přihlášen.");
        }

        var cart = await cartRepository.GetCartForCurrentUserAsync(user.Id, cancellationToken);
        if (cart is null)
        {
            logger.LogWarning("Není možné odebrat položku z košíku – košík uživatele s ID {UserId} nebyl nalezen.", user.Id);
            throw new NotFoundException(nameof(Cart), user.Id);
        }

        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            logger.LogWarning("Není možné odebrat položku z košíku – produkt s ID {ProductId} nebyl nalezen.", request.ProductId);
            throw new NotFoundException(nameof(Product), request.ProductId);
        }

        var existingCartItem = await cartItemRepository.GetForUserAndProductAsync(
            user.Id,
            product.Id,
            cancellationToken);

        if (existingCartItem is null)
        {
            logger.LogWarning("Nelze odebrat položku {ProductName} z košíku uživatele {UserEmail} – položka v košíku neexistuje.",
                product.Name, user.Email);
            throw new NotFoundException(nameof(CartItem), request.ProductId);
        }

        if (request.Quantity > existingCartItem.Quantity)
        {
            logger.LogWarning(
                "Nelze odebrat {Quantity} ks položky {ProductName} z košíku uživatele {UserEmail} – v košíku je pouze {CurrentQty} ks.",
                request.Quantity, product.Name, user.Email, existingCartItem.Quantity);

            throw new ForbiddenException("Nelze odebrat více kusů, než je aktuálně v košíku.");
        }

        await cartItemRepository.DecreaseQuantityAsync(existingCartItem, request.Quantity, cancellationToken);

        if (existingCartItem.Quantity == 0)
        {
            await cartItemRepository.DeleteForCurrentUserAsync(existingCartItem, cancellationToken);
        }

        await productRepository.AddStockQtyAsync(product, request.Quantity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Z košíku uživatele {UserEmail} bylo odebráno {Quantity} ks položky {ProductName}.",
            user.Email, request.Quantity, product.Name);
    }
}
