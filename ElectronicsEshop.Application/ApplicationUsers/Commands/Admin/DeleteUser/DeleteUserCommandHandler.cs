using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.DeleteUser;

public sealed class DeleteUserCommandHandler(
    ILogger<DeleteUserCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    ICartItemRepository cartItemRepository) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation("Začínám deaktivovat uživatele s Id {UserId}.", request.Id);

        var user = await userManager.FindByIdAsync(request.Id);

        if (user is null)
        {
            logger.LogWarning("Uživatel s Id {UserId} nebyl nalezen, nelze jej deaktivovat.", request.Id);
            throw new NotFoundException(nameof(ApplicationUser), request.Id);
        }

        if (!user.Active)
        {
            logger.LogWarning("Uživatel s Id {UserId} a emailem {Email} je již deaktivní. Deaktivace nebude provedena.",  request.Id, user.Email);
            throw new ConflictException("již deaktivní", "uživatel", "Nastal konflikt");
        }

        if (await cartItemRepository.ExistsForUserAsync(user.Id, cancellationToken))
        {
            logger.LogWarning("Nelze deaktivovat uživatele s E-mailem {UserEmail} – má v košíku položky. Je třeba je odstranit.", user.Email); throw new ForbiddenException($"Nepodařilo se deaktivovat uživatele s E-mailem: {user.Email}, v košíku jsou položky, které je třeba odstranit.");
        }

        user.Active = false;
        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("Při deaktivaci uživatele {UserId} ({Email}) došlo k chybě: {Errors}", user.Id, user.Email,  errors);
            throw new DomainException($"Uživatel nelze deaktivovat: {errors}");
        }

        logger.LogInformation("Uživatel {UserId} ({Email}) byl úspěšně deaktivován.", user.Id, user.Email);
    }
}
