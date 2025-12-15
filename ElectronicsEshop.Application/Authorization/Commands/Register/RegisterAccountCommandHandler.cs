using AutoMapper;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.Enums;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Authorization.Commands.Register;

public sealed class RegisterAccountCommandHandler(UserManager<ApplicationUser> userManager,
    ILogger<RegisterAccountCommandHandler> logger,
    ICartRepository cartRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterAccountCommand>
{
    public async Task Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = mapper.Map<ApplicationUser>(request);
        user.Active = true;

        var result = await userManager.CreateAsync(user, request.Password);

        if(!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("Při vytváření uživatele: {Username} nastala chyba: {Error}", user.UserName, errorMessage);
            throw new DomainException($"Chyba při vytváření účtu: {errorMessage}");
        }

        var roleResult = await userManager.AddToRoleAsync(user, UserRoles.Client.ToString());

        if(!roleResult.Succeeded)
        {
            var roleErrorMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            logger.LogError("Při vytváření uživatele: {UserName} nastala chyba: {Error}", user.UserName, roleErrorMessage);
            throw new DomainException($"Chyba při vytváření role: {roleErrorMessage}");
        }

        await cartRepository.CreateAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Uživatel: {FullName} byl vytvořen", user.FullName);
    }
}
