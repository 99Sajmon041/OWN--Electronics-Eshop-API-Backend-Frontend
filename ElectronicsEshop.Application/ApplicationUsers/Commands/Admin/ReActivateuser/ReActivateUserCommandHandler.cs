using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.ReActivateuser;

public sealed class ReActivateUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    ILogger<ReActivateUserCommandHandler> logger) : IRequestHandler<ReActivateUserCommand>
{
    public async Task Handle(ReActivateUserCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            logger.LogWarning("Uživatel s ID: {UserId} nebyl nalezen.", request.UserId);
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);
        }

        if(user.Active)
        {
            logger.LogWarning("Uživatel s ID: {UserId} je již aktivní.", request.UserId);
            throw new ConflictException("už aktivní", "uživatel", "Nastal konflikt");
        }

        user.Active = true;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("Chyba při aktivaci uživatele {UserId}: {Errors}", request.UserId, errors);
            throw new DomainException("Nepodařilo se aktivovat uživatele.");
        }

        logger.LogInformation("Uživatel s ID: {UserId} byl aktivován.", request.UserId);
    }
}
