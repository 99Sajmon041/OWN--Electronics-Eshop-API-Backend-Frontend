namespace ElectronicsEshop.Blazor.Auth
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginRequest request);
        Task LogoutAsync();
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
