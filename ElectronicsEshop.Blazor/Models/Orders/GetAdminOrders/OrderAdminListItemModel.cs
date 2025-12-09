using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Blazor.Models.Orders.GetAdminOrders;

public sealed class OrderAdminListItemModel
{
    public int Id { get; set; }
    public string CustomerEmail { get; set; } = default!;
    public string CustomerFullName { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int ItemsCount { get; set; }
}
