using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Orders.GetAdminOrder;
using ElectronicsEshop.Blazor.Models.Orders.GetAdminOrders;
using ElectronicsEshop.Blazor.Utils;
using ElectronicsEshop.Domain.Enums;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Orders;

public sealed class OrderAdminService(HttpClient httpClient) : IOrderAdminService
{
    public async  Task<PagedResult<OrderAdminListItemModel>> GetAllAsync(OrdersAdminRequest request, CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["page"] = request.Page.ToString(),
            ["pageSize"] = request.PageSize.ToString(),
            ["from"] = request.From?.ToString("yyyy-MM-dd"),
            ["to"] = request.To?.ToString("yyyy-MM-dd"),
            ["orderStatus"] = request.OrderStatus?.ToString(),
            ["customerEmail"] = request.CustomerEmail,
            ["orderId"] = request.OrderId?.ToString(),
            ["userId"] = request.UserId
        };

        var filtered = query
            .Where(kpv => !string.IsNullOrEmpty(kpv.Value))
            .ToDictionary(kpv => kpv.Key, kpv => kpv.Value);

        var url = QueryHelpers.AddQueryString("api/admin/orders", filtered);

        var response = await httpClient.GetAsync(url, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se načíst objednávky.");
            throw new ApplicationException(message);
        }


        var data = await response.Content.ReadFromJsonAsync<PagedResult<OrderAdminListItemModel>>(ct) 
            ?? throw new ApplicationException("Nepodařilo se načíst objednávky.");

        if (!data.Items.Any())
        {
            return data ?? new PagedResult<OrderAdminListItemModel>
            {
                Items = [],
                TotalCount = 0,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        return data;
    }

    public async Task UpdateStatusAsync(int orderId, OrderStatus orderStatus, CancellationToken ct = default)
    {
        var body = new
        {
            NewStatus = orderStatus
        };

        var response = await httpClient.PatchAsJsonAsync($"api/admin/orders/{orderId}/changestatus", body, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se urpavit stav objednávky.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task<OrderAdminModel> GetAsync(int orderId, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"api/admin/orders/{orderId}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat objednávku.");
            throw new ApplicationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<OrderAdminModel>(ct);

        return data ?? throw new ApplicationException("Nepodařilo se získat objednávku.");
    }
}
