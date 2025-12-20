using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Application.Abstractions;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string userId, string token, CancellationToken cancellationToken);
    Task SendOrderConfirmationEmailAsync(ApplicationUser user, Order order, CancellationToken cancellationToken);
}
