using AutoMapper;
using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Queries.Self.GetProfile;

public sealed class GetProfileQueryHandler(
    IUserContext userContext,
    UserManager<ApplicationUser> userManager,
    IOrderRepository orderRepository,
    ILogger<GetProfileQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetProfileQuery, ApplicationUserDto>
{
    public async Task<ApplicationUserDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = userContext.GetCurrentUser();

        if (currentUser is null)
        {
            logger.LogWarning("Nelze načíst aktuálně přihlášeného uživatele – uživatel není autentizovaný.");
            throw new UnauthorizedException("Uživatel není přihlášen.");
        }

        var user = await userManager.FindByIdAsync(currentUser.Id);

        if (user is null)
        {
            logger.LogWarning("Uživatel s E-mailem {UserEmail} nebyl nalezen.", currentUser.Email);
            throw new NotFoundException(nameof(ApplicationUser), currentUser.Email);
        }

        var appUser = mapper.Map<ApplicationUserDto>(user);
        appUser.OrdersCount = await orderRepository.GetOrdersCountForUserAsync(user.Id, cancellationToken);

        logger.LogInformation("Profil uživatele {UserEmail} byl úspěšně načten.", currentUser.Email);

        return appUser;
    }
}
