namespace ElectronicsEshop.Blazor.Models.Orders.Self.GetOrder;

public sealed class OrderItemModel
{
    public int Id { get; set; }
    public string ProductName { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}
