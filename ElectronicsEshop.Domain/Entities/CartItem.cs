namespace ElectronicsEshop.Domain.Entities;

public class CartItem
{
    public int Id { get; set; }
    public Product Product { get; set; } = default!;
    public int ProductId { get; set; }
    public Cart Cart { get; set; } = default!;
    public int CartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
