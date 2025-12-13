namespace ElectronicsEshop.Blazor.Models.Common;

public sealed class RequestResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}
