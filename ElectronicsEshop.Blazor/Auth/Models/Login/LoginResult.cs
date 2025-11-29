namespace ElectronicsEshop.Blazor.Auth.Models.Login;

public sealed class LoginResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}
