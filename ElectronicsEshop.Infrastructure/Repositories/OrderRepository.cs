using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.Enums;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsEshop.Infrastructure.Repositories;

public sealed class OrderRepository(AppDbContext db) : IOrderRepository
{
    public async Task<(IReadOnlyList<Order>, int totalCount)> GetPagedForCurrentUserAsync(string userId, int page, int pageSize, CancellationToken ct)
    {
        var baseQuery = db.Orders
            .AsNoTracking()
            .Where(o => o.ApplicationUserId == userId);

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

    public async Task<(IReadOnlyList<Order>, int totalCount)> GetPagedForAdminAsync(int page, int pageSize, DateOnly? from, DateOnly? to, OrderStatus? status, string? customerEmail, int? orderNumber, CancellationToken ct)
    {
        var query = db.Orders
            .AsNoTracking()
            .Include(o => o.ApplicationUser)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .AsQueryable();

        if (from is not null)
            query = query.Where(o => DateOnly.FromDateTime(o.CreatedAt) >= from.Value);

        if (to is not null)
            query = query.Where(o => DateOnly.FromDateTime(o.CreatedAt) <= to.Value);

        if (status is not null)
            query = query.Where(o => o.OrderStatus == status.Value);

        if (!string.IsNullOrWhiteSpace(customerEmail))
            query = query.Where(o => o.ApplicationUser.Email == customerEmail);

        if (orderNumber is not null)
            query = query.Where(o => o.OrderNumber == orderNumber.Value);

        var ordersCount = await query.CountAsync(ct);

        var orders = await query
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

    public async Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus, CancellationToken ct)
    {
        order.OrderStatus = orderStatus;
        db.Update(order);

        await db.SaveChangesAsync(ct);
    }   

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await db.Orders
            .Include(o => o.ApplicationUser)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }
}
