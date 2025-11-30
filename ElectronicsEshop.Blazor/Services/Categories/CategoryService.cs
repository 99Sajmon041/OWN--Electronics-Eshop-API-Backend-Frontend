using ElectronicsEshop.Blazor.Models.Categories;
using ElectronicsEshop.Blazor.Utils;
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
            throw new InvalidOperationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<IReadOnlyList<CategoryModel>>(ct) ?? [];

        return data.ToDictionary(c => c.Id, c => c.Name);
    }
}