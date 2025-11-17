using ElectronicsEshop.Application.Abstractions;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Account.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager,
    IEmailService emailService,
    ILogger<ForgotPasswordCommandHandler> logger) : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Email))
        {
            logger.LogWarning("Požadavek na reset hesla přišel s prázdným nebo neplatným emailem.");
            return;
        }

        logger.LogInformation("Zpracovávám požadavek na reset hesla pro email {Email}.", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);

        if(user is null)
        {
            logger.LogInformation("Požadavek na reset hesla pro email {Email}, ale uživatel nebyl nalezen.", request.Email);
            return;
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        await emailService.SendPasswordResetEmailAsync(user.Email!, user.Id, token, cancellationToken);

        logger.LogInformation("Reset hesla – pokud účet pro {Email} existuje, byl odeslán e-mail s instrukcemi.", request.Email);
    }
}
