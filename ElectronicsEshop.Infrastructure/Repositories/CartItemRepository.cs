using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsEshop.Infrastructure.Repositories;

public sealed class CartItemRepository(AppDbContext db) : ICartItemRepository
{
    public async Task<bool> ExistsForProductAsync(int productId, CancellationToken cancellationToken)
    {
        return await db.CartItems
            .AsNoTracking()
            .AnyAsync(ci => ci.ProductId == productId);
    }

    public async Task<bool> ExistsWithCategoryAsync(int categoryId, CancellationToken ct)
    {
        return await db.CartItems
            .AsNoTracking()
            .Include(ci => ci.Product)
            .ThenInclude(p => p.Category)
            .AnyAsync(ci => ci.Product.CategoryId == categoryId, ct);
    }
}
