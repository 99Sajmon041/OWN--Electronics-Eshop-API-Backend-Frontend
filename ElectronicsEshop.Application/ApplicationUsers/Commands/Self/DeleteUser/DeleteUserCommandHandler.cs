using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Self.DeleteUser;

public sealed class DeleteUserCommandHandler(IUserContext userContext,
    UserManager<ApplicationUser> userManager,
    ILogger<DeleteUserCommandHandler> logger,
    ICartItemRepository cartItemRepository) : IRequestHandler<DeleteUserCommand>
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

        var user = await userManager.FindByIdAsync(currentUser.Id);
        if (user is null)
        {
            logger.LogWarning("Uživatel s E-mailem {UserEmail} nebyl nalezen při pokusu o deaktivaci profilu.", currentUser.Email);
            throw new NotFoundException(nameof(ApplicationUser), currentUser.Email);
        }

        var pwdResult = await userManager.CheckPasswordAsync(user, request.Password);
        if(!pwdResult)
        {
            logger.LogWarning("Nelze deaktivovat uživatele s E-mailem {UserEmail}. Bylo zadáno špatné heslo.", user.Email);
            throw new DomainException("Nepodařilo se deaktivovat účet. Zkontrolujte zadané heslo.");
        }

        if (await cartItemRepository.ExistsForUserAsync(user.Id, cancellationToken))
        {
            logger.LogWarning("Nelze deaktivovat uživatele s E-mailem {UserEmail} – má v košíku položky. Je třeba je odstranit.", user.Email);
            throw new ForbiddenException($"Nepodařilo se deaktivovat uživatele s E-mailem: {user.Email}, v košíku jsou položky, které je třeba odstranit.");
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
