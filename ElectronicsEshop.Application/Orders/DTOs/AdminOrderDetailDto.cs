using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Application.Orders.DTOs;

public sealed class AdminOrderDetailDto
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public string CustomerId { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerFullName { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int ItemsCount { get; set; }
    public ICollection<AdminOrderItemDto> OrderItems { get; set; } = new List<AdminOrderItemDto>();
}

