namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface ICartItemRepository
{
    Task<bool> ExistsForProductAsync(int productId, CancellationToken ct);
    Task<bool> ExistsWithCategoryAsync(int categoryId, CancellationToken ct);
}
