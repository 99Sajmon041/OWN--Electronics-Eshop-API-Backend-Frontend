namespace ElectronicsEshop.Blazor.Models.Payments;

public sealed class PaymentRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? UserId { get; set; }
    public int? OrderId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}
