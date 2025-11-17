using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.UpdateUserRole;

public sealed class UpdateUserRoleCommandHandler(
    UserManager<ApplicationUser> userManager,
    ILogger<UpdateUserRoleCommandHandler> logger) : IRequestHandler<UpdateUserRoleCommand>
{
    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(ApplicationUser), request.Id);

        var roles = await userManager.GetRolesAsync(user);

        if (roles.Count == 0)
        {
            var addResult = await userManager.AddToRoleAsync(user, request.Role);

            if (!addResult.Succeeded)
            {
                var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                logger.LogWarning("Nepodařilo se nastavit roli {Role} uživateli {Email}. Chyby: {Errors}", request.Role, user.Email, errors);

                throw new DomainException($"Roli uživatele nelze změnit: {errors}");
            }

            logger.LogInformation("Uživateli s e-mailem {UserMail} byla nastavena nová role: {UserRole}", user.Email,  request.Role);

            return;
        }

        if (roles.Count > 1)
        {
            logger.LogWarning("Uživatel {Email} má neočekávaně více rolí: {Roles}", user.Email, string.Join(", ", roles));

            throw new DomainException("Uživatel má přiřazeno více rolí, operaci nelze provést.");
        }

        var currentRole = roles.First();

        if (currentRole == request.Role)
        {
            throw new ConflictException(request.Role, "role");
        }

        var removeResult = await userManager.RemoveFromRoleAsync(user, currentRole);

        if (!removeResult.Succeeded)
        {
            var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
            logger.LogWarning("Nepodařilo se odebrat roli {Role} uživateli {Email}. Chyby: {Errors}", currentRole, user.Email, errors);

            throw new DomainException($"Roli uživatele nelze změnit: {errors}");
        }

        var addRoleResult = await userManager.AddToRoleAsync(user, request.Role);

        if (!addRoleResult.Succeeded)
        {
            var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
            logger.LogWarning("Nepodařilo se nastavit novou roli {Role} uživateli {Email}. Chyby: {Errors}", request.Role, user.Email, errors);

            throw new DomainException($"Roli uživatele nelze změnit: {errors}");
        }

        logger.LogInformation("Uživateli s e-mailem {UserMail} byla změněna role z {OldRole} na {NewRole}.", user.Email, currentRole, request.Role);
    }
}
