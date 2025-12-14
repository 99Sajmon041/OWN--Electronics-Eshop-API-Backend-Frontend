using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Orders.Admin.GetAdminOrder;
using ElectronicsEshop.Blazor.Models.Orders.Admin.GetAdminOrders;
using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Blazor.Services.Orders;

public interface IOrderAdminService
{
    Task<PagedResult<OrderListItemModel>> GetAllAsync(OrdersRequest request, CancellationToken ct = default);
    Task UpdateStatusAsync(int orderId, OrderStatus orderStatus, CancellationToken ct = default);
    Task<OrderModel> GetAsync(int orderId, CancellationToken ct = default);
}
