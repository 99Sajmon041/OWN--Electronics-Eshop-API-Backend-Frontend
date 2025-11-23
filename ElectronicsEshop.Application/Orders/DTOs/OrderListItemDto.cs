namespace ElectronicsEshop.Application.Orders.DTOs;

public sealed class OrderListItemDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int ItemsCount { get; set; }
}