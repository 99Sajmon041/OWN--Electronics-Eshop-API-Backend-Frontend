using AutoMapper;
using ElectronicsEshop.Application.ApplicationUsers.Commands.Self.UpdateUser;
using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public sealed class UpdateProfileCommandHandler(
    IUserContext userContext,
    ILogger<UpdateProfileCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IRequestHandler<UpdateProfileCommand, ApplicationUserDto>
{
    public async Task<ApplicationUserDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = userContext.GetCurrentUser();

        if (currentUser is null)
        {
            logger.LogWarning("Nelze aktualizovat profil – uživatel není autentizovaný.");
            throw new UnauthorizedException("Uživatel není přihlášen.");
        }

        var user = await userManager.FindByEmailAsync(currentUser.Email);

        if (user is null)
        {
            logger.LogWarning("Uživatel s E-mailem {UserEmail} nebyl nalezen při pokusu o aktualizaci profilu.", currentUser.Email);
            throw new NotFoundException(nameof(ApplicationUser), currentUser.Email);
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.DateOfBirth = request.DateOfBirth;
        user.PhoneNumber = request.PhoneNumber;

        user.Address = mapper.Map<Address>(request.Address);

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("Nepodařilo se aktualizovat profil uživatele {UserEmail}. Chyby: {Errors}", user.Email, errors);

            throw new DomainException("Nepodařilo se aktualizovat profil uživatele.");
        }

        var dto = mapper.Map<ApplicationUserDto>(user);
        return dto;
    }
}
