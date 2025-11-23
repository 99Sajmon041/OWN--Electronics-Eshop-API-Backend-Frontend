using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsEshop.Infrastructure.Repositories
{
    public sealed class OrderItemRepository(AppDbContext db) : IOrderItemRepository
    {
        public async Task<bool> ExistsForProductAsync(int productId, CancellationToken cancellationToken)
        {
            return await db.OrderItems
                .AsNoTracking()
                .AnyAsync(oi => oi.ProductId == productId, cancellationToken);
        }

        public async Task<bool> ExistsWithCategoryAsync(int categoryId, CancellationToken ct)
        {
            return await db.OrderItems
                .AsNoTracking()
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .AnyAsync(oi => oi.Product.CategoryId == categoryId, ct);
        }
    }
}
