using AutoMapper;
using ElectronicsEshop.Application.Carts.DTOs;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Carts.Queries.Self.GetCart;

public sealed class GetCartQueryHandler(
    ILogger<GetCartQueryHandler> logger,
    ICartRepository cartRepository,
    IMapper mapper,
    IUserContext userContext) : IRequestHandler<GetCartQuery, CartDetailDto>
{
    public async Task<CartDetailDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = userContext.GetCurrentUser();

        if(user is null)
        {
            logger.LogWarning("Není možné ukázat košík, uživatel nebyl nalezen.");
            throw new NotFoundException(nameof(ApplicationUser), "ID");
        }

        var cart = await cartRepository.GetCartForCurrentUserAsync(user.Id, cancellationToken);
        var cartDto = mapper.Map<CartDetailDto>(cart);

        return cartDto;
    }
}
