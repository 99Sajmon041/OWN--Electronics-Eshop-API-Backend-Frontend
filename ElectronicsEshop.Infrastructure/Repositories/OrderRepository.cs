using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.Enums;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsEshop.Infrastructure.Repositories;

public sealed class OrderRepository(AppDbContext db) : IOrderRepository
{
    public async Task<(IReadOnlyList<Order>, int totalCount)> GetPagedForCurrentUserAsync(string userId, int page, int pageSize, OrderStatus? orderStatus, CancellationToken ct)
    {
        var baseQuery = db.Orders
            .AsNoTracking()
            .Where(o => o.ApplicationUserId == userId);

        if(orderStatus is not null)
            baseQuery = baseQuery.Where(o => o.OrderStatus == orderStatus);

        var totalCount = await baseQuery.CountAsync(ct);

        var items = await baseQuery
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<Order?> GetByIdForCurrentUserAsync(int id, string userId, CancellationToken ct)
    {
        return await db.Orders
            .AsNoTracking()
            .Include(o => o.ApplicationUser)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.ApplicationUserId == userId, ct);
    }

    public async Task<(IReadOnlyList<Order>, int totalCount)> GetPagedForAdminAsync(int page, int pageSize, int? orderId, string? userId, DateOnly? from, DateOnly? to, OrderStatus? status, string? customerEmail, CancellationToken ct)
    {
        var query = db.Orders
            .AsNoTracking()
            .Include(o => o.ApplicationUser)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .AsQueryable();

        if (orderId is not null)
            query = query.Where(o => o.Id == orderId);

        if (!string.IsNullOrWhiteSpace(userId))
            query = query.Where(o => o.ApplicationUserId == userId);

        if (from is not null)
        {
            var fromDt = from.Value.ToDateTime(TimeOnly.MinValue);
            query = query.Where(o => o.CreatedAt >= fromDt);
        }

        if (to is not null)
        {
            var toExclusive = to.Value.AddDays(1).ToDateTime(TimeOnly.MinValue);
            query = query.Where(o => o.CreatedAt < toExclusive);
        }

        if (status is not null)
            query = query.Where(o => o.OrderStatus == status.Value);

        if (!string.IsNullOrWhiteSpace(customerEmail))
            query = query.Where(o => o.ApplicationUser.Email!.Contains(customerEmail.Trim()));

        var ordersCount = await query.CountAsync(ct);

        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .ThenByDescending(o => o.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (orders, ordersCount);
    }

    public async Task<Order?> GetByIdForAdminAsync(int id, CancellationToken ct)
    {
        return await db.Orders
            .AsNoTracking()
            .Include(o => o.ApplicationUser)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public Task UpdateOrderStatusAsync(int orderId, OrderStatus status, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var stub = new Order { Id = orderId };
        db.Orders.Attach(stub);
        stub.OrderStatus = status;
        db.Entry(stub).Property(x => x.OrderStatus).IsModified = true;
        return Task.CompletedTask;
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await db.Orders
            .Include(o => o.ApplicationUser)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public  Task CreateAsync(Order order, CancellationToken ct)
    {
        return db.Orders.AddAsync(order, ct).AsTask();
    }

    public async Task<int> GetOrdersCountForUserAsync(string userId, CancellationToken ct)
    {
        return await db.Orders
            .AsNoTracking()
            .Where(o => o.ApplicationUserId == userId)
            .CountAsync(ct);
    }
}
