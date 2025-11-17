using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;

namespace ElectronicsEshop.Infrastructure.Repositories;

public sealed class CartRepository(AppDbContext db) : ICartRepository
{
    public async Task CreateAsync(string userId, CancellationToken cancellationToken)
    {
        var entity = new Cart
        {
            ApplicationUserId = userId,
            UpdatedAt = DateTime.UtcNow,
            CartItems = new List<CartItem>()
        };

        await db.Carts.AddAsync(entity);
        await db.SaveChangesAsync(cancellationToken);
    }
}
