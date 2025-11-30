namespace ElectronicsEshop.Blazor.Models.Auth.Login;

public sealed class LoginResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}
