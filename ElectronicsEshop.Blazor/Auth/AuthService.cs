using Blazored.LocalStorage;
using ElectronicsEshop.Blazor.Auth.Constants;
using ElectronicsEshop.Blazor.Auth.Models.ForgotPassword;
using ElectronicsEshop.Blazor.Auth.Models.Login;
using ElectronicsEshop.Blazor.Auth.Models.Register;
using ElectronicsEshop.Blazor.Auth.Models.ResetPassword;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Auth
{
    public sealed class AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider) : IAuthService
    {
        public async Task<LoginResult> LoginAsync(LoginRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);

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

                await localStorage.SetItemAsStringAsync(TokenConstant.TokenStorageKey, loginResponse.AccessToken);

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

        public async Task LogoutAsync()
        {
            await localStorage.RemoveItemAsync(TokenConstant.TokenStorageKey);

            httpClient.DefaultRequestHeaders.Authorization = null;

            if (authStateProvider is ApiAuthenticationStateProvider apiAuth)
            {
                apiAuth.MarkUserAsLoggedOut();
            }
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);
            
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

        public async Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/forgot-password", request);

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

        public async Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/aut/reset-password", request);

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
