using ElectronicsEshop.Domain.Entities;

namespace ElectronicsEshop.Application.Abstractions;

public sealed class PaymentResult
{
    public bool Success { get; init; }
    public Payment? Payment { get; set; }
    public string? Error { get; init; }
}
