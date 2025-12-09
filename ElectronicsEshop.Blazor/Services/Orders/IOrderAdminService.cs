using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Orders.GetAdminOrders;

namespace ElectronicsEshop.Blazor.Services.Orders;

public interface IOrderAdminService
{
    Task<PagedResult<OrderAdminListItemModel>> GetAllAsync(OrdersAdminRequest request, CancellationToken ct = default);
}
