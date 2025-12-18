using Blazored.LocalStorage;
using ElectronicsEshop.Blazor.Models.Auth.ForgotPassword;
using ElectronicsEshop.Blazor.Models.Auth.Login;
using ElectronicsEshop.Blazor.Models.Auth.Register;
using ElectronicsEshop.Blazor.Models.Auth.ResetPassword;
using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Constants;
using ElectronicsEshop.Blazor.Services.Carts;
using ElectronicsEshop.Blazor.UI.Message;
using ElectronicsEshop.Blazor.UI.State;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Auth
{
    public sealed class AuthService(
        HttpClient httpClient, 
        ILocalStorageService localStorage,
        AuthenticationStateProvider authStateProvider,
        ICartsService cartsService,
        MessageService messageService) : IAuthService
    {
        public async Task<RequestResult> LoginAsync(LoginModel model, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", model, ct);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (loginResponse is null || string.IsNullOrWhiteSpace(loginResponse.AccessToken))
                {
                    return new RequestResult
                    {
                        Success = false,
                        ErrorMessage = "Server nevrátil platný přihlašovací token."
                    };
                }

                await localStorage.SetItemAsStringAsync(TokenConstant.TokenStorageKey, loginResponse.AccessToken, ct);

                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);

                if (authStateProvider is ApiAuthenticationStateProvider apiAuth)
                {
                    await apiAuth.MarkUserAsAuthenticatedAsync();
                }

                try
                {
                    await cartsService.SetCartStateAsync(ct);
                }
                catch
                {
                    messageService.ShowError("Nepodařilo se načíst lištu s košíkem.");
                }

                return new RequestResult { Success = true };
            }

            var message = await response.ReadProblemMessageAsync("Došlo k neočekávané chybě při přihlášení.");

            return new RequestResult
            {
                Success = false,
                ErrorMessage = message
            };
        }

        public async Task LogoutAsync(CancellationToken ct = default)
        {
            if (authStateProvider is ApiAuthenticationStateProvider apiAuth)
            {
                try
                {
                    await cartsService.DeleteAllItemsAsync(ct: ct);
                }
                catch
                {

                }

                await apiAuth.LogoutAsync();
            }
        }

        public async Task<RegisterResult> RegisterAsync(RegisterModel model, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", model, ct);
            
            if(response.IsSuccessStatusCode)
            {
                return new RegisterResult { Success = true };
            }

            var message = await response.ReadProblemMessageAsync("Došlo k neočekávané chybě při registraci.");

            return new RegisterResult
            {
                Success = false,
                ErrorMessage = message
            };
        }

        public async Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordModel model, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/forgot-password", model, ct);

            if(response.IsSuccessStatusCode)
            {
                return new ForgotPasswordResult
                {
                    Success = true
                };
            }

            var message = await response.ReadProblemMessageAsync("Došlo k neočekávané chybě při žádosti o reset hesla.");

            return new ForgotPasswordResult
            {
                Success = false,
                ErrorMessage = message
            };
        }

        public async Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordModel model, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/reset-password", model, ct);

            if (response.IsSuccessStatusCode)
            {
                return new ResetPasswordResult
                {
                    Success = true
                };
            }

            var message = await response.ReadProblemMessageAsync("Došlo k neočekávané chybě při změně hesla.");

            return new ResetPasswordResult
            {
                Success = false,
                ErrorMessage = message
            };
        }
    }
}
