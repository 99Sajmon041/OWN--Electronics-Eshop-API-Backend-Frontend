using ElectronicsEshop.Blazor.Models.Categories.CreateCategory;
using ElectronicsEshop.Blazor.Models.Categories.GetCategories;
using ElectronicsEshop.Blazor.Models.Categories.GetCategory;
using ElectronicsEshop.Blazor.Models.Categories.UpdateCategory;
using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Categories;

public sealed class CategoryService(HttpClient httpClient) : ICategoryService
{
    public async Task<IDictionary<int, string>> GetAllCategiresAsync(CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync("api/admin/categories/all", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se načíst kategorie.");
            throw new KeyNotFoundException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<IReadOnlyList<CategoryModel>>(ct) ?? [];

        return data.ToDictionary(c => c.Id, c => c.Name);
    }

    public async Task CreateAsync(CreateCategoryModel model, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/admin/categories", model, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se vytvořit kategorii.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task<PagedResult<CategoryModel>> GetAllAsync(CategoryPageRequest request, CancellationToken ct = default)
    {
        var query = new Dictionary<string, string>
        {
            ["page"] = request.Page.ToString(),
            ["pageSize"] = request.PageSize.ToString()
        };

        var filtered = query
            .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        var url = QueryHelpers.AddQueryString("api/admin/categories", filtered);

        var response = await httpClient.GetAsync(url, ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se načíst kategorie.");
            throw new KeyNotFoundException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<PagedResult<CategoryModel>>(cancellationToken: ct);

        return data ?? new PagedResult<CategoryModel>
        {
            Items = Array.Empty<CategoryModel>(),
            TotalCount = 0,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<CategoryModel> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"api/admin/categories/{id}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync($"Nepodařilo se získat kategorii s ID: {id}");
            throw new KeyNotFoundException(message);
        }

        var category = await response.Content.ReadFromJsonAsync<CategoryModel>(cancellationToken: ct);

        return category  ?? throw new KeyNotFoundException($"Nepodařilo se získat kategorii s ID: {id}");
    }

    public async Task UpdateAsync(UpdateCategoryModel model, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsJsonAsync($"api/admin/categories/{model.Id}", model, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se upravit kategorii.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"api/admin/categories/{id}", ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync($"Nepodařilo se vymazat kategorii s ID: {id}");
            throw new InvalidOperationException(message);
        }
    }
}