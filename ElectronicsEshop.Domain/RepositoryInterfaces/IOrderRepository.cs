using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface IOrderRepository
{
    Task<(IReadOnlyList<Order>, int totalCount)> GetPagedForCurrentUserAsync(string userId, int page, int pageSize, CancellationToken ct);
    Task<Order?> GetByIdForCurrentUserAsync(int id, string userId, CancellationToken ct);
    Task<(IReadOnlyList<Order>, int totalCount)> GetPagedForAdminAsync(int page, int pageSize, int? orderId, DateOnly? from, DateOnly? to, OrderStatus? status, string? customerEmail, CancellationToken ct);
    Task<Order?> GetByIdForAdminAsync(int id, CancellationToken ct);
    Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus, CancellationToken ct);
    Task<Order?> GetByIdAsync(int id, CancellationToken ct);
    Task CreateAsync(Order order, CancellationToken ct);
    Task<int> GetOrdersCountForUserAsync(string userId, CancellationToken ct);
}
    