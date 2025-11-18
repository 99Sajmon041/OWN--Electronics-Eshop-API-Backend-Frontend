using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Self.DeleteUser;

public sealed class DeleteUserCommandHandler(IUserContext userContext,
    UserManager<ApplicationUser> userManager,
    ILogger<DeleteUserCommandHandler> logger) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = userContext.GetCurrentUser();

        if(currentUser is null)
        {
            logger.LogWarning("Nelze odstranit profil – uživatel není autentizovaný.");
            throw new UnauthorizedException("Uživatel není přihlášen.");
        }

        var user = await userManager.FindByEmailAsync(currentUser.Email);

        if (user is null)
        {
            logger.LogWarning("Uživatel s E-mailem {UserEmail} nebyl nalezen při pokusu o smazání profilu.", currentUser.Email);
            throw new NotFoundException(nameof(ApplicationUser), currentUser.Email);
        }

        user.Active = false;
        var deleteResult = await userManager.UpdateAsync(user);
        
        if(!deleteResult.Succeeded)
        {
            var errors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));

            logger.LogError("Chyba při deaktivaci uživatele {UserEmail}: {Errors}", user.Email, errors);
            throw new DomainException("Nepodařilo se deaktivovat uživatele.");
        }

        logger.LogWarning("Uživatel s E-mailem: {UserEmail} byl deaktivován.", user.Email);
    }
}
