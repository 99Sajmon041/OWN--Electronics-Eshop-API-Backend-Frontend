using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Products.CreateAdminProduct;
using ElectronicsEshop.Blazor.Models.Products.GetAdminProducts;
using ElectronicsEshop.Blazor.Models.Products.Shared;
using ElectronicsEshop.Blazor.Models.Products.UpdateAdminProduct;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Products;

public sealed class ProductAdminService(HttpClient httpClient) : IProductAdminService
{
    public async Task<PagedResult<ProductModel>> GetAllAsync(ProductAdminRequest request, CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["page"] = request.Page.ToString(),
            ["pageSize"] = request.PageSize.ToString(),
            ["sort"] = request.Sort,
            ["order"] = request.Order.ToString(),
            ["categoryId"] = request.CategoryId?.ToString(),
            ["isActive"] = request.IsActive?.ToString(),
            ["priceMin"] = string.Empty,
            ["priceMax"] = string.Empty,
            ["stockMin"] = string.Empty,
            ["stockMax"] = string.Empty,
            ["q"] = request.Q
        };

        var filtered = query
            .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!);

        var url = QueryHelpers.AddQueryString("api/products", filtered);

        var response = await httpClient.GetAsync(url, ct);

        if(!response.IsSuccessStatusCode)
        {
                var message = await response.ReadProblemMessageAsync("Nepodařilo se načíst produkty.");
                throw new KeyNotFoundException(message);
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

    public async Task CreateAsync(CreateProductModel request, CancellationToken ct = default)
    {
        if(request.ImageFile is null)
        {
            throw new InvalidOperationException("Musíte vybrat obrázek.");
        }

        using var form = new MultipartFormDataContent();

        var file = request.ImageFile;
        var fileContent = new StreamContent(file.OpenReadStream(5 * 1024 * 1024, ct));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");

        form.Add(fileContent, "ImageFile", file.Name);

        form.Add(new StringContent(request.ProductCode), "Data.ProductCode");
        form.Add(new StringContent(request.IsActive.ToString()), "Data.IsActive");
        form.Add(new StringContent(request.Name), "Data.Name");
        form.Add(new StringContent(request.Description), "Data.Description");
        form.Add(new StringContent(request.CategoryId.ToString()), "Data.CategoryId");
        form.Add(new StringContent(request.Price.ToString(CultureInfo.InvariantCulture)), "Data.Price");
        form.Add(new StringContent(request.DiscountPercentage.ToString(CultureInfo.InvariantCulture)), "Data.DiscountPercentage");
        form.Add(new StringContent(request.StockQty.ToString()), "Data.StockQty");

        var response = await httpClient.PostAsync("api/products", form, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se vytvořit produkt.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task UpdateStockQtyAsync(int productId, int newStockQty, CancellationToken ct = default)
    {
        var body = new { StockQty = newStockQty };

        var response = await httpClient.PatchAsJsonAsync($"api/products/{productId}/stock-qty", body, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se doskladnit požadované množství produktu.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task SetActiveAsync(int productId, bool isActive, CancellationToken ct = default)
    {
        var body = new { IsActive  = isActive};

        var response = await httpClient.PatchAsJsonAsync($"api/products/{productId}/active", body, ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se změnit stav produktu.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task UpdateDiscountAsync(int productId, decimal newDiscount, CancellationToken ct = default)
    {
        var body = new { DiscountPercentage = newDiscount };

        var response = await httpClient.PatchAsJsonAsync($"api/products/{productId}/discount", body, ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se upravit slevu na produkt.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task UpdateImageAsync(int productId, IBrowserFile file, CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent();

        var fileContent = new StreamContent(file.OpenReadStream(5 * 1024 * 1024, ct));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");

        form.Add(fileContent, "ImageFile", file.Name);

        var response = await httpClient.PatchAsync($"api/products/{productId}/image", form, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se změnit obrázek.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task DeleteAsync(int productId, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"api/products/{productId}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Produkt nelze odstranit.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task UpdateAsync(int productId, UpdateProductModel model, CancellationToken ct = default)
    {
        var body = new
        {
            Id = productId,
            Data = new
            {
                model.ProductCode,
                model.IsActive,
                model.Name,
                model.Description,
                model.CategoryId,
                model.Price,
                model.DiscountPercentage,
                model.StockQty,
                model.ImageUrl
            }
        };

        var response = await httpClient.PutAsJsonAsync($"api/products/{productId}", body, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Produkt se nepodařilo se upravit.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task<ProductModel> GetByIdAsync(int productId, CancellationToken ct = default)
    {
        var url = $"api/products/{productId}";

        var response = await httpClient.GetAsync(url, ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat produkt.");
            throw new InvalidOperationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<ProductModel>(ct);

        return data ?? throw new KeyNotFoundException("Produkt nebyl nalezen.");
    }
}
