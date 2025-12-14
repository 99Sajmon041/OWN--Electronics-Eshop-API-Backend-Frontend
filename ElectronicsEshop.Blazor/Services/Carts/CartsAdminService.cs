using ElectronicsEshop.Blazor.Models.Carts.Shared;
using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Carts
{
    public class CartsAdminService(HttpClient httpClient) : ICartsAdminService
    {
        public async Task<PagedResult<CartModel>> GetAllAsync(CommonPageRequest request, CancellationToken ct = default)
        {
            var query = new Dictionary<string, string>
            {
                ["page"] = request.Page.ToString(),
                ["pageSize"] = request.PageSize.ToString(),
                ["email"] = request.Email ?? string.Empty
            };

            var filtered = query
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .ToDictionary(x => x.Key, x => x.Value);

            var url = QueryHelpers.AddQueryString("api/admin/carts", filtered);

            var response = await httpClient.GetAsync(url, ct);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.ReadProblemMessageAsync("Nepodařilo se získat košíky uživatelů.");
                throw new InvalidOperationException(message);
            }

            var data = await response.Content.ReadFromJsonAsync<PagedResult<CartModel>>(ct);

            return data ?? new PagedResult<CartModel>
            {
                Items = [],
                TotalCount = 0,
                Page = 1,
                PageSize = 5
            };
        }
    }
}
