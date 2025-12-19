using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface IPaymentRepository
{
    Task CreatePaymentAsync(Payment payment, CancellationToken ct);
    Task UpdatePaymentAsync(int paymentId, int orderId, DateTime updatedAt, CancellationToken ct);
    Task<(IReadOnlyList<Payment> payments, int totalCount)> GetPagedAsync(int page, int pageSize, string? userId, int? orderId, DateTime? from, DateTime?  to, CancellationToken ct);
}
