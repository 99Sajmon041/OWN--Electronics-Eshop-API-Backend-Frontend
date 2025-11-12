using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface IProductRepository
{
    Task<(IReadOnlyList<Product> Items, int totalCount)> GetPagedAsync(
        int page, int pageSize, string? sort, bool asc,string? q, int? categoryId, bool? isActive,
        decimal? priceMin, decimal? priceMax, int? stockMin, int? stockMax, CancellationToken ct = default);
    Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken ct = default);
    Task<int> AddAsync(Product product, CancellationToken ct = default);
    Task UpdateAsync(Product product, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsByProductCodeAsync(string productCode, CancellationToken ct);
    Task<Product?> GetByProductCodeAsync(string productCode, CancellationToken ct);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct);
}
