using ElectronicsEshop.Blazor.Models.Categories.CreateCategory;
using ElectronicsEshop.Blazor.Models.Categories.GetCategory;
using ElectronicsEshop.Blazor.Models.Categories.UpdateCategory;
using ElectronicsEshop.Blazor.Models.Common;

namespace ElectronicsEshop.Blazor.Services.Categories;

public interface ICategoryService
{
    Task<PagedResult<CategoryModel>> GetAllAsync(CategoryRequest request, CancellationToken ct = default);
    Task<IDictionary<int, string>> GetAllCategiresAsync(CancellationToken ct = default);
    Task<CategoryModel> GetByIdAsync(int id, CancellationToken ct = default);
    Task CreateAsync(CreateCategoryModel model, CancellationToken ct = default);
    Task UpdateAsync(UpdateCategoryModel model, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
