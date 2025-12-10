using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Orders.GetAdminOrder;
using ElectronicsEshop.Blazor.Models.Orders.GetAdminOrders;
using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Blazor.Services.Orders;

public interface IOrderAdminService
{
    Task<PagedResult<OrderAdminListItemModel>> GetAllAsync(OrdersAdminRequest request, CancellationToken ct = default);
    Task UpdateStatusAsync(int orderId, OrderStatus orderStatus, CancellationToken ct = default);
    Task<OrderAdminModel> GetAsync(int orderId, CancellationToken ct = default);
}
