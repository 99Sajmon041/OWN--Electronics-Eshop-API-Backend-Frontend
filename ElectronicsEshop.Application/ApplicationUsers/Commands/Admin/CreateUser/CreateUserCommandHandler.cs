using AutoMapper;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.CreateUser;

public sealed class CreateUserCommandHandler(UserManager<ApplicationUser> userManager,
    ILogger<CreateUserCommandHandler> logger,
    ICartRepository cartRepository,
    IMapper mapper) : IRequestHandler<CreateUserCommand, string>
{
    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<ApplicationUser>(request);
        user.Active = true;
        logger.LogInformation("Začínám vytvářet uživatele s emailem {Email} a rolí {Role}.", request.Email, request.Role);

        var createResult = await userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            logger.LogWarning("Nepodařilo se vytvořit uživatele s emailem {Email}. Chyby: {Errors}", request.Email, errors);
            throw new DomainException($"Chyba při vytvoření uživatele: {errors}");
        }

        var roleResult = await userManager.AddToRoleAsync(user, request.Role);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            logger.LogWarning("Uživatel {Email} byl vytvořen, ale nepodařilo se přiřadit roli {Role}. Chyby: {Errors}", request.Email, request.Role, errors);
            await userManager.DeleteAsync(user);
            throw new DomainException($"Chyba při přiřazení role uživateli: {errors}");
        }

        await cartRepository.CreateAsync(user.Id, cancellationToken);

        logger.LogInformation( "Uživatel {UserId} ({Email}) byl úspěšně vytvořen s rolí {Role}.", user.Id, user.Email, request.Role);

        return user.Id;
    }
}
