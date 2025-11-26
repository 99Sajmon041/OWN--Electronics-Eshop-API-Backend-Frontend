using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

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

        await db.Carts.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Cart>, int)> GetAllCartsForAdminAsync(string? userId, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Carts
            .Include(c => c.CartItems)
            .Include(c => c.ApplicationUser)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(c => c.ApplicationUserId == userId);

        var cartsCount = await query.CountAsync(ct);

        var carts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (carts, cartsCount);            
    }

    public async Task<Cart?> GetCartForCurrentUserAsync(string userId, CancellationToken ct)
    {
        return await db.Carts
            .AsNoTracking()
            .Include(c => c.ApplicationUser)
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.ApplicationUserId == userId, ct);
    }
}
