using ElectronicsEshop.Blazor.Auth.Models.ForgotPassword;
using ElectronicsEshop.Blazor.Auth.Models.Login;
using ElectronicsEshop.Blazor.Auth.Models.Register;
using ElectronicsEshop.Blazor.Auth.Models.ResetPassword;

namespace ElectronicsEshop.Blazor.Auth
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(LoginRequest request);
        Task LogoutAsync();
        Task<RegisterResult> RegisterAsync(RegisterRequest request);
        Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
