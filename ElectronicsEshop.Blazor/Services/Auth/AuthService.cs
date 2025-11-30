using Blazored.LocalStorage;
using ElectronicsEshop.Blazor.Models.Auth.ForgotPassword;
using ElectronicsEshop.Blazor.Models.Auth.Login;
using ElectronicsEshop.Blazor.Models.Auth.Register;
using ElectronicsEshop.Blazor.Models.Auth.ResetPassword;
using ElectronicsEshop.Blazor.Models.Constants;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Auth
{
    public sealed class AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider) : IAuthService
    {
        public async Task<LoginResult> LoginAsync(LoginModel request, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request, ct);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (loginResponse is null || string.IsNullOrWhiteSpace(loginResponse.AccessToken))
                {
                    return new LoginResult
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

                return new LoginResult { Success = true };
            }

            var message = await response.ReadProblemMessageAsync("Došlo k neočekávané chybě při přihlášení.");

            return new LoginResult
            {
                Success = false,
                ErrorMessage = message
            };
        }

        public async Task LogoutAsync(CancellationToken ct = default)
        {
            await localStorage.RemoveItemAsync(TokenConstant.TokenStorageKey, ct);

            httpClient.DefaultRequestHeaders.Authorization = null;

            if (authStateProvider is ApiAuthenticationStateProvider apiAuth)
            {
                apiAuth.MarkUserAsLoggedOut();
            }
        }

        public async Task<RegisterResult> RegisterAsync(RegisterModel request, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request, ct);
            
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

        public async Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordModel request, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/forgot-password", request, ct);

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

        public async Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordModel request, CancellationToken ct = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/aut/reset-password", request, ct);

            if(response.IsSuccessStatusCode)
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
