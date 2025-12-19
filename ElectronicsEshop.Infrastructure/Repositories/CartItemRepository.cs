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
            .AnyAsync(ci => ci.ProductId == productId, cancellationToken);
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
    }

    public async Task<CartItem?> GetForUserAndProductAsync(string userId, int productId, CancellationToken ct)
    {
        return await db.CartItems.FirstOrDefaultAsync(ci => ci.Cart.ApplicationUserId == userId && ci.ProductId == productId, ct);
    }

    public Task IncreaseQuantityAsync(CartItem cartItem, int quantity, CancellationToken ct)
    {
        cartItem.Quantity += quantity;
        db.CartItems.Update(cartItem);
        return Task.CompletedTask;
    }

    public Task DecreaseQuantityAsync(CartItem cartItem, int quantity, CancellationToken ct)
    {
        cartItem.Quantity -= quantity;
        db.CartItems.Update(cartItem);
        return Task.CompletedTask;
    }

    public async Task DeleteAllForCurrentUserAsync(string id, CancellationToken ct)
    {
        var items = await db.CartItems.Where(ci => ci.Cart.ApplicationUserId == id).ToListAsync(cancellationToken: ct);
        db.CartItems.RemoveRange(items);
    }

    public async Task<CartItem?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await db.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == id, ct);
    }

    public Task DeleteForCurrentUserAsync(CartItem cartItem, CancellationToken ct)
    {
        db.CartItems.Remove(cartItem);
        return Task.CompletedTask;
    }

    public async Task<int> GetCartItemsCountForCurrentUser(string userId, CancellationToken ct)
    {
        var sum = await db.CartItems
            .AsNoTracking()
            .Where(ci => ci.Cart.ApplicationUserId == userId)
            .Select(ci => (int?)ci.Quantity)
            .SumAsync(ct);

        return sum ?? 0;
    }
}
