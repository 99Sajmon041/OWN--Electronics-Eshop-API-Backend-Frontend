using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Self.UpdateUserPassword;

public sealed class UpdateUserPasswordCommandHandler(IUserContext userContext,
    UserManager<ApplicationUser> userManager,
    ILogger<UpdateUserPasswordCommandHandler> logger) : IRequestHandler<UpdateUserPasswordCommand>
{
    public async Task Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = userContext.GetCurrentUser();

        if(currentUser is null)
        {
            logger.LogWarning("Nelze aktualizovat heslo – uživatel není autentizovaný.");
            throw new UnauthorizedException("Uživatel není přihlášen.");
        }

        var user = await userManager.FindByEmailAsync(currentUser.Email);
        
        if(user == null)
        {
            logger.LogWarning("Uživatel s E-mailem {UserEmail} nebyl nalezen při pokusu o změnu hesla.", currentUser.Email);
            throw new NotFoundException(nameof(ApplicationUser), currentUser.Email);
        }

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        if(!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("Nepodařilo se změnit heslo uživatele {UserEmail}. Chyby: {Errors}", user.Email, errors);

            throw new DomainException("Nepodařilo se změnit heslo uživatele.");
        }

        logger.LogInformation("Heslo uživatele {UserEmail} bylo úspěšně změněno.", user.Email);
    }
}
