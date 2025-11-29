using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ElectronicsEshop.Blazor.Auth
{
    public sealed class AuthService(HttpClient httpClient,  ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider) : IAuthService
    {
        private const string TokenStorageKey = "authToken";

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
                return false;

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (loginResponse is null || string.IsNullOrWhiteSpace(loginResponse.AccessToken))
                return false;

            await localStorage.SetItemAsStringAsync(TokenStorageKey, loginResponse.AccessToken);

            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);

            if(authStateProvider is ApiAuthenticationStateProvider apiAuth)
            {
                await apiAuth.MarkUserAsAuthenticatedAsync();
            }

            return true;
        }

        public async Task LogoutAsync()
        {
            await localStorage.RemoveItemAsync(TokenStorageKey);

            httpClient.DefaultRequestHeaders.Authorization = null;

            if(authStateProvider is ApiAuthenticationStateProvider apiAuth)
            {
                apiAuth.MarkUserAsLoggedOut();
            }
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);
            return response.IsSuccessStatusCode;
        }
    }
}
