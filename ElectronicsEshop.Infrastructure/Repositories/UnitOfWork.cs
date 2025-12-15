using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;

namespace ElectronicsEshop.Infrastructure.Repositories;

public sealed class UnitOfWork(AppDbContext db, ICartItemRepository cartItems, ICartRepository carts, ICategoryRepository categoreis, IOrderItemRepository orderItems, IOrderRepository orders, IPaymentRepository payments, IProductRepository products) : IUnitOfWork
{
    private readonly AppDbContext _db = db;

    public ICartItemRepository CartItems { get; } = cartItems;
    public ICartRepository Carts { get; } = carts;
    public ICategoryRepository Categories { get; } = categoreis;
    public IOrderItemRepository OrderItems { get; } = orderItems;
    public IOrderRepository Orders { get; } = orders;
    public IPaymentRepository Payments { get; } = payments;
    public IProductRepository Products { get; } = products;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _db.SaveChangesAsync(ct);
    }
}
