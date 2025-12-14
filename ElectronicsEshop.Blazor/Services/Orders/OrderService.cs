using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Orders.Self.GetOrder;
using ElectronicsEshop.Blazor.Models.Orders.Self.GetOrders;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Orders;

public sealed class OrderService(HttpClient httpClient) : IOrderService
{
    public async Task<PagedResult<OrderListItemModel>> GetAllAsync(CommonPageRequest request, CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["page"] = request.Page.ToString(),
            ["pageSize"] = request.PageSize.ToString(),
            ["orderStatus"] = request.OrderStatus?.ToString()
        };

        var filtered = query
            .Where(kpv => !string.IsNullOrEmpty(kpv.Value))
            .ToDictionary(kpv => kpv.Key, kpv => kpv.Value);

        var url = QueryHelpers.AddQueryString("api/orders", filtered);

        var response = await httpClient.GetAsync(url, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat objednávky.");
            throw new ApplicationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<PagedResult<OrderListItemModel>>(ct);

        return data ?? throw new ApplicationException("Nepodařilo se získat objednávky.");
    }

    public async Task<OrderModel> GetByIdAsync(int orderId, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"api/orders/{orderId}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat objednávku.");
            throw new ApplicationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<OrderModel>(ct);

        return data ?? throw new ApplicationException("Nepodařilo se získat objednávku.");
    }
}
