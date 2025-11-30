using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Products.CreateProduct;
using ElectronicsEshop.Blazor.Models.Products.GetProducts;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Products;

public sealed class ProductAdminService(HttpClient httpClient) : IProductAdminService
{
    public async Task<PagedResult<ProductListItemModel>> GetAllAsync(ProductRequest request, CancellationToken ct = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["page"] = request.Page.ToString(),
            ["pageSize"] = request.PageSize.ToString(),
            ["sort"] = request.Sort,
            ["order"] = request.Order.ToString(),
            ["categoryId"] = request.CategoryId?.ToString(),
            ["isActive"] = request.IsActive?.ToString(),
            ["priceMin"] = request.PriceMin?.ToString(CultureInfo.InvariantCulture),
            ["priceMax"] = request.PriceMax?.ToString(CultureInfo.InvariantCulture),
            ["stockMin"] = request.StockMin?.ToString(),
            ["stockMax"] = request.StockMax?.ToString(),
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
                throw new InvalidOperationException(message);
        }

        var data = await httpClient.GetFromJsonAsync<PagedResult<ProductListItemModel>>(url, ct);

        return data ?? new PagedResult<ProductListItemModel>
        { 
            Items = [],
            TotalCount = 0,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task CreateAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        if(request.ImageFile is null)
        {
            throw new InvalidOperationException("Musíte vybrat obrázek.");
        }

        using var form = new MultipartFormDataContent();

        var file = request.ImageFile;
        var fileContent = new StreamContent(file.OpenReadStream(5 * 1024 * 1024));
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");

        form.Add(fileContent, "ImageFile", file.Name);

        form.Add(new StringContent(request.ProductCode), "Data.ProductCode");
        form.Add(new StringContent(request.IsActive.ToString()), "Data.IsActive");
        form.Add(new StringContent(request.Name), "Data.Name");
        form.Add(new StringContent(request.Description), "Data.Description");
        form.Add(new StringContent(request.CategoryId.ToString()), "Data.CategoryId");
        form.Add(new StringContent(request.Price.ToString(CultureInfo.InvariantCulture)), "Data.Price");
        form.Add(new StringContent(request.DiscountPercentage.ToString(CultureInfo.InvariantCulture)), "Data.DiscountPercentage");
        form.Add(new StringContent(request.StockQty.ToString()), "Data.StockQty");

        form.Add(new StringContent(string.Empty), "Data.ImageUrl");

        var response = await httpClient.PostAsync("api/products", form, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se vytvořit produkt.");
            throw new InvalidOperationException(message);
        }
    }
}
