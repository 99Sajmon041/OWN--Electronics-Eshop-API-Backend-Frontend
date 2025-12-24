using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Role.Queries.GetRole;

public sealed class GetRoleQueryHandler(
    ILogger<GetRoleQueryHandler> logger,
    UserManager<ApplicationUser> userManager) : IRequestHandler<GetRoleQuery, string>
{
    public async Task<string> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByIdAsync(request.Id);
        if(user is null)
        {
            logger.LogWarning("Uživatel s ID: {UserId} nebyl nalezen, zjištění role není možné.", request.Id);
            throw new NotFoundException(nameof(ApplicationUser), request.Id);
        }

        var role = await userManager.GetRolesAsync(user);
        if (role is null || !role.Any())
        {
            logger.LogWarning("Uživatel s ID: {UserId} nemá přiřazenou žádnou roli.", request.Id);
            throw new NotFoundException("Role uživatele", user.FullName);
        }

        return role.First();
    }
}
