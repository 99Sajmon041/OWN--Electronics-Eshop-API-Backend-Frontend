using Blazored.LocalStorage;
using ElectronicsEshop.Blazor.Auth.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ElectronicsEshop.Blazor.Auth;

public sealed class ApiAuthenticationStateProvider(
    ILocalStorageService localStorage,
    HttpClient httpClient) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>(TokenConstant.TokenStorageKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        ClaimsIdentity identity;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        }
        catch
        {
            await localStorage.RemoveItemAsync(TokenConstant.TokenStorageKey);
            httpClient.DefaultRequestHeaders.Authorization = null;
            identity = new ClaimsIdentity();
        }

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticatedAsync()
    {
        var authState = await GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}
