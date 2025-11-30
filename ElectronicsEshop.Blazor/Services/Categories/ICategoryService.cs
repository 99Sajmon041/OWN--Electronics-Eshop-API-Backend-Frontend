namespace ElectronicsEshop.Blazor.Services.Categories;

public interface ICategoryService
{
    Task<IDictionary<int, string>> GetAllCategiresAsync(CancellationToken ct = default);
}
