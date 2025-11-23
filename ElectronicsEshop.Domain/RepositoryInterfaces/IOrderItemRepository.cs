namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface IOrderItemRepository
{
    Task<bool> ExistsForProductAsync(int productId, CancellationToken cancellationToken);
    Task<bool> ExistsWithCategoryAsync(int categoryId, CancellationToken ct);
}
