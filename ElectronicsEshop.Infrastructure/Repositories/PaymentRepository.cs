using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsEshop.Infrastructure.Repositories;

public sealed class PaymentRepository(AppDbContext db) : IPaymentRepository
{
    public async Task CreatePaymentAsync(Payment payment, CancellationToken ct)
    {
        await db.Payments.AddAsync(payment, ct);
    }

    public async Task UpdatePaymentAsync(int paymentId, int orderId, DateTime updatedAt, CancellationToken ct)
    {
        var payment = await FindPaymentByIdAsync(paymentId, ct);
        if (payment is null)
            return;

        payment.OrderId = orderId;
        payment.UpdatedAt = updatedAt;
        db.Update(payment);
    }

    private async Task<Payment?> FindPaymentByIdAsync(int paymentId, CancellationToken ct)
    {
        return await db.Payments.FindAsync(new object?[] { paymentId }, ct);
    }

    public async Task<(IReadOnlyList<Payment> payments, int totalCount)> GetPagedAsync(int page, int pageSize, string? userId, int? orderId, DateTime? from, DateTime? to, CancellationToken ct)
    {
        var query = db.Payments
               .AsNoTracking()
               .AsQueryable();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            var trimmed = userId.Trim();

            query = trimmed.Length >= 32
                ? query.Where(p => p.UserId == trimmed)
                : query.Where(p => p.UserId.StartsWith(trimmed));
        }

        if (orderId.HasValue)
            query = query.Where(p => p.OrderId == orderId.Value);

        if (from.HasValue)
        {
            var fromDate = from.Value.Date;
            query = query.Where(p => p.CreatedAt >= fromDate);
        }

        if (to.HasValue)
        {
            var toExclusive = to.Value.Date.AddDays(1);
            query = query.Where(p => p.CreatedAt < toExclusive);
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
