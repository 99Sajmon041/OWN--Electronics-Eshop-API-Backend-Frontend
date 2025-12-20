using ElectronicsEshop.Application.Abstractions;
using ElectronicsEshop.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ElectronicsEshop.Infrastructure.Email;

public sealed class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;
    public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task SendPasswordResetEmailAsync(string email, string userId, string token, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var encodedToken = WebUtility.UrlEncode(token);
        var encodedUserId = WebUtility.UrlEncode(userId);

        var resetUrl = $"{_settings.FrontendBaseUrl}/reset-password?userId={encodedUserId}&token={encodedToken}";

        var subject = "Obnovení hesla - Electronics E-shop";

        var bodyBuilder = new StringBuilder();
        bodyBuilder.AppendLine("Dobrý den,");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("obdrželi jsme požadavek na obnovení hesla k Vašemu účtu v Electronics E-shopu.");
        bodyBuilder.AppendLine("Pro změnu hesla klikněte na následující odkaz:");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine(resetUrl);
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("Pokud jste o změnu hesla nežádali, tento e-mail prosím ignorujte.");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("S pozdravem");
        bodyBuilder.AppendLine("Tým Electronics E-shop");

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress, _settings.FromName),
            Subject = subject,
            Body = bodyBuilder.ToString(),
            IsBodyHtml = false,
        };

        mailMessage.To.Add(email);

        using var SmtpClient = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = new NetworkCredential(_settings.UserName, _settings.Password)
        };

        try
        {
            _logger.LogInformation("Odeslání e-maile pro reset hesla na adresu {Email}.", email);

            await SmtpClient.SendMailAsync(mailMessage, cancellationToken);

            _logger.LogInformation("E-mail pro reset hesla byl úspěšně odeslán na {Email}.", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Při odesílání e-mailu pro reset hesla na {Email} došlo k chybě.", email);
            throw;
        }
    }

    public async Task SendOrderConfirmationEmailAsync(ApplicationUser user, Order order, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var subject = $"Electronics E-shop – potvrzení objednávky č. {order.Id}";

        var contentAddressBuilder = new StringBuilder();
        contentAddressBuilder.AppendLine("Doručovací adresa:");
        contentAddressBuilder.AppendLine($"{user.FullName}");
        contentAddressBuilder.AppendLine($"{user.Address.Street} {user.Address.NumberOfHouse}");
        contentAddressBuilder.AppendLine($"{user.Address.PostalCode}, {user.Address.Town}");
        contentAddressBuilder.AppendLine();

        var contentOrderBuilder = new StringBuilder();
        contentOrderBuilder.AppendLine("Souhrn objednávky:");
        contentOrderBuilder.AppendLine();

        foreach (var item in order.OrderItems)
        {
            var lineTotal = item.UnitPrice * item.Quantity;
            contentOrderBuilder.AppendLine($"- {item.Product.Name} | {item.Quantity}x | {lineTotal:0.00} Kč");
        }

        contentOrderBuilder.AppendLine();
        contentOrderBuilder.AppendLine("-------------------------------------------------------");
        contentOrderBuilder.AppendLine($"Celková cena: {order.TotalAmount:0.00} Kč");

        var bodyBuilder = new StringBuilder();
        bodyBuilder.AppendLine("Dobrý den,");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("děkujeme za Vaši objednávku v Electronics E-shopu.");
        bodyBuilder.AppendLine();

        var messageBuilder = new StringBuilder();
        messageBuilder.Append(bodyBuilder);
        messageBuilder.Append(contentAddressBuilder);
        messageBuilder.Append(contentOrderBuilder);
        messageBuilder.AppendLine();
        messageBuilder.AppendLine();
        messageBuilder.AppendLine("S pozdravem");
        messageBuilder.AppendLine("Tým Electronics E-shop");

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress, _settings.FromName),
            Subject = subject,
            Body = messageBuilder.ToString(),
            IsBodyHtml = false,
        };

        mailMessage.To.Add(user.Email!);

        using var smtpClient = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = new NetworkCredential(_settings.UserName, _settings.Password)
        };

        _logger.LogInformation("Odesílám e-mail s potvrzením objednávky {OrderId} na {Email}.", order.Id, user.Email);

        await smtpClient.SendMailAsync(mailMessage, cancellationToken);

        _logger.LogInformation("E-mail s potvrzením objednávky {OrderId} byl odeslán na {Email}.", order.Id, user.Email);
    }
}
