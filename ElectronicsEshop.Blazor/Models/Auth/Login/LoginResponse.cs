namespace ElectronicsEshop.Blazor.Models.Auth.Login;

public sealed class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
