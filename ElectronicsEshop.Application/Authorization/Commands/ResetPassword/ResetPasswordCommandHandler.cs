using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Authorization.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager,
    ILogger<ResetPasswordCommandHandler> logger) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation("Zpracovávám požadavek na reset hesla pro uživatele s Id {UserId}.", request.UserId);

        var user = await userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            logger.LogWarning("Reset hesla selhal – uživatel s Id {UserId} nebyl nalezen. Pravděpodobně neplatný nebo expirovaný odkaz.", request.UserId);

            throw new DomainException("Odkaz pro reset hesla je neplatný nebo vypršel.");
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            logger.LogWarning("Reset hesla pro uživatele {UserId} ({Email}) selhal. Chyby: {Errors}", user.Id, user.Email, errors);

            throw new DomainException($"Heslo nelze resetovat: {errors}");
        }

        logger.LogInformation("Heslo uživatele {UserId} ({Email}) bylo úspěšně resetováno.",  user.Id,  user.Email);
    }
}
