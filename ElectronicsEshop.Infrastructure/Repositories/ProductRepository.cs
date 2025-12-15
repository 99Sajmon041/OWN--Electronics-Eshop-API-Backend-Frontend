using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsEshop.Infrastructure.Repositories;

public sealed class ProductRepository(AppDbContext db) : IProductRepository
{
    public async Task<(IReadOnlyList<Product> Items, int totalCount)> GetPagedAsync(
        int page, int pageSize, string? sort,
        bool asc, string? q,int? categoryId, bool? isActive, decimal? priceMin, decimal? priceMax,
        int? stockMin, int? stockMax, CancellationToken ct = default)
    {
        var query = db.Products.AsNoTracking().Include(p => p.Category).AsQueryable();

        if (categoryId is int cid) query = query.Where(p => p.CategoryId == cid);
        if (isActive is bool act) query = query.Where(p => p.IsActive == act);
        if (priceMin is decimal pmin) query = query.Where(p => p.Price >= pmin);
        if (priceMax is decimal pmax) query = query.Where(p => p.Price <= pmax);
        if (stockMin is int smin) query = query.Where(p => p.StockQty >= smin);
        if (stockMax is int smax) query = query.Where(p => p.StockQty <= smax);

        if(!string.IsNullOrWhiteSpace(q))
        {
            var t = q.Trim().ToLower();

            query = query.Where(p =>
                p.Name.ToLower().Contains(t) ||
                p.ProductCode.ToLower().Contains(t) ||
                (p.Description != null && p.Description.ToLower().Contains(t)));
        }

        switch ((sort ?? "name").ToLowerInvariant())
        {
            case "price": 
                query = asc ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                break;

            case "stockqty":
                query = asc ? query.OrderBy(p => p.StockQty) : query.OrderByDescending(p => p.StockQty);
                break;

            case "productcode":
                query = asc ? query.OrderBy(p => p.ProductCode) : query.OrderByDescending(p => p.ProductCode);
                break;

            default:
                query = asc ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                break;
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return(items, total);
    }

    public async Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken ct = default)
    {
        return await db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public Task AddAsync(Product product, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        db.Products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        db.Products.Update(product);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id, CancellationToken ct = default)
    {
        db.Products.Remove(new Product { Id = id });
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByProductCodeAsync(string productCode, CancellationToken ct)
    {
        return await db.Products.AnyAsync(p => p.ProductCode == productCode, ct);
    }

    public async Task<Product?> GetByProductCodeAsync(string productCode, CancellationToken ct)
    {
        var entity = await db.Products.FirstOrDefaultAsync(p => p.ProductCode == productCode, ct);
        return entity;
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public Task UpdateDiscountAsync(Product product, decimal value, CancellationToken ct)
    {
        product.DiscountPercentage = value;
        return Task.CompletedTask;
    }

    public Task AddStockQtyAsync(Product product, int amount, CancellationToken ct)
    {
        product.StockQty += amount;
        return Task.CompletedTask;
    }

    public  Task SetStateOfProductAsync(Product product, bool isActive, CancellationToken ct)
    {
        product.IsActive = isActive;
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsWithCategoryAsync(int categoryId, CancellationToken ct)
    {
        return await db.Products
            .AsNoTracking()
            .AnyAsync(p => p.CategoryId == categoryId, ct);
    }
}