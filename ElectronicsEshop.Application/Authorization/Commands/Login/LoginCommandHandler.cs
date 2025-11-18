using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Authorization.Commands.Login;

public sealed class LoginCommandHandler(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommand, bool>
{
    public async Task<bool> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            logger.LogWarning("Pokus o přihlášení s neexistujícím e-mailem: {Email}", request.Email);
            throw new DomainException("Neplatné přihlašovací údaje.");
        }

        if (!user.Active)
        {
            logger.LogWarning("Pokus o přihlášení deaktivovaného uživatele: {Email}", request.Email);
            throw new DomainException("Účet je deaktivován.");
        }

        var result = await signInManager.PasswordSignInAsync(user.UserName!, request.Password, request.RememberMe, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                logger.LogWarning("Uživatel {Email} je uzamčen.", request.Email);
                throw new DomainException("Účet je dočasně zablokován. Zkuste to prosím později.");
            }

            logger.LogWarning("Neplatné přihlašovací údaje pro uživatele {Email}.", request.Email);
            throw new DomainException("Neplatné přihlašovací údaje.");
        }

        logger.LogInformation("Uživatel {Email} byl úspěšně přihlášen.", request.Email);
        return true;
    }
}
