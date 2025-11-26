using ElectronicsEshop.Domain.Entities;
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

    public async Task<bool> ExistsForUserAsync(string id, CancellationToken ct)
    {
        return await db.CartItems.AnyAsync(ci => ci.Cart.ApplicationUserId == id, ct);
    }

    public async Task AddAsync(CartItem cartItem, CancellationToken ct)
    {
        await db.AddAsync(cartItem, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task<CartItem?> GetForUserAndProductAsync(string userId, int productId, CancellationToken ct)
    {
        return await db.CartItems.FirstOrDefaultAsync(ci => ci.Cart.ApplicationUserId == userId && ci.ProductId == productId, ct);
    }

    public async Task UpdateQuantityAsync(CartItem cartItem, int quantity, CancellationToken ct)
    {
        cartItem.Quantity += quantity;
        db.CartItems.Update(cartItem);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAllForCurrentUserAsync(string id, CancellationToken ct)
    {
        var items = await db.CartItems.Where(ci => ci.Cart.ApplicationUserId == id).ToListAsync(cancellationToken: ct);
        db.CartItems.RemoveRange(items);

        await db.SaveChangesAsync(ct);
    }

    public async Task<CartItem?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await db.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == id, ct);
    }

    public async Task DeleteForCurrentUserAsync(CartItem cartItem, CancellationToken ct)
    {
        db.CartItems.Remove(cartItem);
        await db.SaveChangesAsync(ct);
    }
}
