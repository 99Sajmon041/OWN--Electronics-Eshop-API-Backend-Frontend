using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Payments;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;

namespace ElectronicsEshop.Blazor.Services.Payments;

public sealed class PaymentsService(HttpClient httpClient, JsonSerializerOptions jsonOptions) : IPaymentsService
{
    public async Task<PagedResult<PaymentModel>> GetAllAsync(PaymentRequest request, CancellationToken ct = default)
    {
        var query = new Dictionary<string, string>
        {
            ["page"] = request.Page.ToString(),
            ["pageSize"] = request.PageSize.ToString(),
            ["userId"] = request.UserId ?? "",
            ["orderId"] = request.OrderId?.ToString() ?? "",
            ["from"] = request.From?.ToString("yyyy-MM-dd") ?? "",
            ["to"] = request.To?.ToString("yyyy-MM-dd") ?? ""
        };

        var filtered = query
            .Where(x => !string.IsNullOrWhiteSpace(x.Value))
            .ToDictionary(x => x.Key, x => x.Value);

        var url = QueryHelpers.AddQueryString("api/adminpayments", filtered);

        var response = await httpClient.GetAsync(url, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat platby");
            throw new HttpRequestException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<PagedResult<PaymentModel>>(jsonOptions, ct);

        return data ?? new PagedResult<PaymentModel>
        {
            Items = [],
            TotalCount = 0,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }
}
