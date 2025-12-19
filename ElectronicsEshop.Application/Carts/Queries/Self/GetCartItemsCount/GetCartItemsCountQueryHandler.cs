using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Carts.Queries.Self.GetCartItemsCount;

public sealed class GetCartItemsCountQueryHandler(
    ILogger<GetCartItemsCountQueryHandler> logger,
    ICartItemRepository cartItemRepository,
    IUserContext userContext) : IRequestHandler<GetCartItemsCountQuery, int>
{
    public async Task<int> Handle(GetCartItemsCountQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = userContext.GetCurrentUser();
        if(user is null)
        {
            logger.LogInformation("Není možné získat aktuálního uživatele.");
            throw new UnauthorizedException("Není možné získat aktuálního uživatele.");
        }

        var cartItemsCount = await cartItemRepository.GetCartItemsCountForCurrentUser(user.Id, ct);
        logger.LogInformation("uživatel: {UserId} získal počet všech položek svého košíku: {Count}.", user.Id, cartItemsCount);

        return cartItemsCount;
    }
}
