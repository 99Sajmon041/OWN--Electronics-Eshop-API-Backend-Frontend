namespace ElectronicsEshop.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public Product Product { get; set; } = default!;
    public int ProductId { get; set; }
    public Order Order { get; set; } = default!;
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
