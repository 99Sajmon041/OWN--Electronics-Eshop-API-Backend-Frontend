namespace ElectronicsEshop.Application.Abstractions;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string userId, string token, CancellationToken cancellationToken);
}
