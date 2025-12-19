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
        var paymentRecord = await FindPaymentByIdAsync(paymentId, ct);
        if (paymentRecord is null)
            return;

        paymentRecord.OrderId = orderId;
        paymentRecord.UpdatedAt = updatedAt;
        db.Update(paymentRecord);
    }

    private async Task<Payment?> FindPaymentByIdAsync(int paymentId, CancellationToken ct)
    {
        return await db.Payments.FindAsync(new object?[] { paymentId }, ct);
    }

    public async Task<(IReadOnlyList<Payment> payments, int totalCount)> GetPagedAsync(int page, int pageSize, string? userId, int? orderId, DateTime? from, DateTime? to, CancellationToken ct)
    {
        var query = db.Payments
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Order)
            .AsQueryable();

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(p => p.UserId == userId);

        if (orderId is not null)
            query = query.Where(p => p.OrderId == orderId);

        if(from is not null)
            query = query.Where(p => p.CreatedAt >= from.Value);

        if (to is not null)
            query = query.Where(p => p.CreatedAt <= to.Value);

        var itemsCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, itemsCount);
    }
}
