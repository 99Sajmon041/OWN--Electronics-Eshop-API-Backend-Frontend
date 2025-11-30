using ElectronicsEshop.Blazor.Models.Auth.ForgotPassword;
using ElectronicsEshop.Blazor.Models.Auth.Login;
using ElectronicsEshop.Blazor.Models.Auth.Register;
using ElectronicsEshop.Blazor.Models.Auth.ResetPassword;

namespace ElectronicsEshop.Blazor.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(LoginModel request, CancellationToken ct = default);
        Task LogoutAsync(CancellationToken ct = default);
        Task<RegisterResult> RegisterAsync(RegisterModel request, CancellationToken ct = default);
        Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordModel request, CancellationToken ct = default);
        Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordModel request, CancellationToken ct = default);
    }
}
