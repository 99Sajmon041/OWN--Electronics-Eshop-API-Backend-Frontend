using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface ICartItemRepository
{
    Task<bool> ExistsForProductAsync(int productId, CancellationToken ct);
    Task<bool> ExistsWithCategoryAsync(int categoryId, CancellationToken ct);
    Task<bool> ExistsForUserAsync(string id, CancellationToken ct);
    Task<CartItem?> GetByIdAsync(int id, CancellationToken ct);
    Task AddAsync(CartItem cartItem, CancellationToken ct);
    Task<CartItem?> GetForUserAndProductAsync(string userId, int productId, CancellationToken ct);
    Task IncreaseQuantityAsync(CartItem cartItem, int quantity, CancellationToken ct);
    Task DecreaseQuantityAsync(CartItem cartItem, int quantity, CancellationToken ct);
    Task DeleteAllForCurrentUserAsync(string id, CancellationToken ct);
    Task DeleteForCurrentUserAsync(CartItem cartItem, CancellationToken ct);
    Task<int> GetCartItemsCountForCurrentUser(string userId, CancellationToken ct);
}
