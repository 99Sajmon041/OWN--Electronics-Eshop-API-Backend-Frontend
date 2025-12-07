using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Products.GetProducts;
using ElectronicsEshop.Blazor.Models.Products.Shared;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Products;

public sealed class ProductService(HttpClient httpClient) : IProductService
{
    public async Task<PagedResult<ProductModel>> GetAllAsync(ProductRequest request, CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["page"] = request.Page.ToString(),
            ["pageSize"] = request.PageSize.ToString(),
            ["categoryId"] = request.CategoryId?.ToString(),
            ["q"] = request.Q
        };

        var filtered = query
            .Where(kpv => !string.IsNullOrWhiteSpace(kpv.Value))
            .ToDictionary(kpv => kpv.Key, kpv => kpv.Value);

        var url = QueryHelpers.AddQueryString("api/products/catalog", filtered);

        var response = await httpClient.GetAsync(url, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se načíst produkty.");
            throw new ApplicationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<PagedResult<ProductModel>>(ct);

        return data ?? new PagedResult<ProductModel>
        {
            Items = [],
            TotalCount = 0,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<ProductModel> GetByIdAsync(int productId, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"api/products/{productId}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se načíst produkt.");
            throw new ApplicationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<ProductModel>(ct);

        return data ?? throw new KeyNotFoundException("Produkt nebyl nalezen.");
    }
}
