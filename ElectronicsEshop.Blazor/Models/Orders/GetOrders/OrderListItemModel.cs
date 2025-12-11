using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Blazor.Models.Orders.GetOrders;

public sealed class OrderListItemModel
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int ItemsCount { get; set; }
}
