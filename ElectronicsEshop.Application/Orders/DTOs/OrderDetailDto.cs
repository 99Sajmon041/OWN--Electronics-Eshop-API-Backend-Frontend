using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Application.Orders.DTOs;

public sealed class OrderDetailDto
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int ItemsCount { get; set; }
    public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}
