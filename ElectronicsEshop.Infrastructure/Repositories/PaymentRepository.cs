using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;

namespace ElectronicsEshop.Infrastructure.Repositories;

public sealed class PaymentRepository(AppDbContext db) : IPaymentRepository
{
    public async Task<int> CreatePaymentAsync(Payment payment, CancellationToken ct)
    {
        await db.Payments.AddAsync(payment, ct);
        await db.SaveChangesAsync(ct);
        return payment.Id;
    }

    public async Task UpdatePaymentAsync(int paymentId, int orderId, DateTime updatedAt, CancellationToken ct)
    {
        var paymentRecord = await FindPaymentByIdAsync(paymentId, ct);
        if (paymentRecord is null)
            return;

        paymentRecord.OrderId = orderId;
        paymentRecord.UpdatedAt = updatedAt;

        await db.SaveChangesAsync(ct);
    }

    private async Task<Payment?> FindPaymentByIdAsync(int paymentId, CancellationToken ct)
    {
        return await db.Payments.FindAsync(new object?[] { paymentId }, ct);
    }
}
