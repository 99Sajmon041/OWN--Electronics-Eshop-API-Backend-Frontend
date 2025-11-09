using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = default!;
    public string ApplicationUserId { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.New;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
