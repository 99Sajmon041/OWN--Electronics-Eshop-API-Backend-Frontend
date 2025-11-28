using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface IPaymentRepository
{
    Task<int> CreatePaymentAsync(Payment payment, CancellationToken ct);
    Task UpdatePaymentAsync(int paymentId, int orderId, DateTime updatedAt, CancellationToken ct);
}
