using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Orders.Self.GetOrder;
using ElectronicsEshop.Blazor.Models.Orders.Self.GetOrders;

namespace ElectronicsEshop.Blazor.Services.Orders;

public interface IOrderService
{
    Task<PagedResult<OrderListItemModel>> GetAllAsync(CommonPageRequest request, CancellationToken ct = default);
    Task<OrderModel> GetByIdAsync(int orderId, CancellationToken ct = default);
}
