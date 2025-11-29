namespace ElectronicsEshop.Application.Authorization.DTOs;

public sealed class LoginResult
{
    public string AccessToken { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}
