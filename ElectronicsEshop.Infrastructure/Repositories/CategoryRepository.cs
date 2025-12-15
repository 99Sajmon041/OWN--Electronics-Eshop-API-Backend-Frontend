using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsEshop.Infrastructure.Repositories;

public sealed class CategoryRepository(AppDbContext db) : ICategoryRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await db.Categories.AnyAsync(c => c.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Category> items, int totalCount)> GetPagedAllAsync(int page, int pageSize, CancellationToken ct)
    {
        var query = db.Categories.AsNoTracking();

        var categoriesCount = await query.CountAsync(ct);

        var categories = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (categories, categoriesCount);
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<int> CreateAsync(Category category, CancellationToken ct)
    {
        await db.Categories.AddAsync(category, ct);
        return category.Id;
    }

    public async Task<bool> ExistByNameAsync(string name, CancellationToken ct)
    {
        return await db.Categories.AnyAsync(x => x.Name == name.Trim(), ct);
    }

    public Task DeleteAsync(int id, CancellationToken ct)
    {
        db.Categories.Remove(new Category { Id = id });
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Category category, CancellationToken ct)
    {
        db.Categories.Update(category);
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct)
    {
        return await db.Categories
            .AsNoTracking()
            .ToListAsync(ct);
    }
}
