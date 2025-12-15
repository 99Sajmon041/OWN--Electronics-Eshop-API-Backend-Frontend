namespace ElectronicsEshop.Domain.RepositoryInterfaces;

public interface  IUnitOfWork
{
    ICartItemRepository CartItems { get; }
    ICartRepository Carts { get; }
    ICategoryRepository Categories { get; }
    IOrderItemRepository OrderItems { get; }
    IOrderRepository Orders { get; }
    IPaymentRepository Payments { get; }
    IProductRepository Products { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
