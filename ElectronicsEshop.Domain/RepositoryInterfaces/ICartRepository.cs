using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface ICartRepository
{
    Task CreateAsync(string userId, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Cart>, int)> GetAllCartsForAdminAsync(string? email, int page, int pageSize, CancellationToken ct);
    Task<Cart?> GetCartForCurrentUserAsync(string userId, CancellationToken ct);
}
