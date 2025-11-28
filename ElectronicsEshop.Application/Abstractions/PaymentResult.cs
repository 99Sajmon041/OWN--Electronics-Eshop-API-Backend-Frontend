namespace ElectronicsEshop.Application.Abstractions;

public sealed class PaymentResult
{
    public bool Success { get; init; }
    public int PaymentId { get; init; }
    public string? Error { get; init; }
}
