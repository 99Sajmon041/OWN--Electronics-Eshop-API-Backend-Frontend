using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface ICategoryRepository
{
    Task<bool> ExistsAsync(int id, CancellationToken ct);
    Task<bool> ExistByNameAsync(string name, CancellationToken ct);
    Task<(IReadOnlyList<Category> items, int totalCount)> GetPagedForCategoriesAsync(int page, int pageSize, CancellationToken ct);
    Task<Category?> GetByIdAsync(int id, CancellationToken ct);
    Task<int> CreateAsync(Category category, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
    Task UpdateAsync(Category category, CancellationToken ct);
}
