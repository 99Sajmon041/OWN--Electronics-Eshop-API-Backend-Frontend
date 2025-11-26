using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Carts.Commands.DeleteCartItems;

public sealed class DeleteCartItemsCommandHandler
    (ILogger<DeleteCartItemsCommandHandler> logger,
    ICartItemRepository cartItemRepository,
    ICartRepository cartRepository,
    IUserContext userContext) : IRequestHandler<DeleteCartItemsCommand>
{
    public async Task Handle(DeleteCartItemsCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = userContext.GetCurrentUser();
        if (user is null)
        {
            logger.LogWarning("Není možné smazat položky z košíku, uživatel nebyl nalezen.");
            throw new NotFoundException(nameof(ApplicationUser), "ID");
        }

        var cart = await cartRepository.GetCartForCurrentUserAsync(user.Id, cancellationToken);
        if (cart is null)
        {
            logger.LogWarning("Není možné smazat položky z košíku, košík uživatele s ID: {UserId} nebyl nalezen.", user.Id);
            throw new NotFoundException(nameof(Cart), $"uživatelským ID: {user.Id}");
        }

        if (cart.CartItems.Count == 0)
        {
            logger.LogWarning("Košík uživatele s e-mailem {UserEmail} je prázdný. Není co mazat.", user.Email);
            throw new ConflictException("Košík je prázdný. Není možné odstranit žádné položky.");
        }

        await cartItemRepository.DeleteAllForCurrentUserAsync(user.Id, cancellationToken);

        logger.LogInformation(
            "Z košíku uživatele s e-mailem {UserEmail} byly odstraněny všechny položky (počet: {CartItemsCount}).",
            user.Email,
            cart.CartItems.Count);
    }
}
