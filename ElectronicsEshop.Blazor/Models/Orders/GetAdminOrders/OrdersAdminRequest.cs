using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Blazor.Models.Orders.GetAdminOrders;

public sealed class OrdersAdminRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public OrderStatus? OrderStatus { get; set; }
    public string? CustomerEmail { get; set; }
    public int? OrderId { get; set; }
    public string? UserId { get; set; }
}
