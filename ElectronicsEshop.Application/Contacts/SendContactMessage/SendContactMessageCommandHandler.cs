using ElectronicsEshop.Application.Abstractions;
using ElectronicsEshop.Application.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Contacts.SendContactMessage;

public sealed class SendContactMessageCommandHandler(
    IEmailService emailService,
    IUserContext userContext,
    ILogger<SendContactMessageCommandHandler> logger) : IRequestHandler<SendContactMessageCommand>
{
    public async Task Handle(SendContactMessageCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = userContext.GetCurrentUser();

        try
        {
            await emailService.SendContactMessageAsync(user?.Id, request.ReplyEmail, request.Subject, request.Message, cancellationToken);

            logger.LogInformation("Kontakt formulář odeslán. UserId={UserId}, ReplyEmail={ReplyEmail}", user?.Id, request.ReplyEmail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Kontakt formulář selhal. UserId={UserId}, ReplyEmail={ReplyEmail}", user?.Id, request.ReplyEmail);
            throw;
        }
    }
}
