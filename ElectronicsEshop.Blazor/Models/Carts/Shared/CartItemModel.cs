namespace ElectronicsEshop.Blazor.Models.Carts.Shared;

public sealed class CartItemModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public string ImageUrl { get; set; } = default!;
}
