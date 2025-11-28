using ElectronicsEshop.Domain.Enums;

namespace ElectronicsEshop.Application.Payments.DTOs;

public sealed class PaymentDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = default!;
    public int? OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
