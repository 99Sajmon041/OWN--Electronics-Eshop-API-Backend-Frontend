using Blazored.LocalStorage;
using ElectronicsEshop.Blazor.Models.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ElectronicsEshop.Blazor.Services.Auth;

public sealed class ApiAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient) : AuthenticationStateProvider
{

    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly AuthenticationState _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>(TokenConstant.TokenStorageKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            return _anonymous;
        }

        JwtSecurityToken jwt;

        try
        {
            jwt = _tokenHandler.ReadJwtToken(token);
        }
        catch
        {
            await LogoutAsync();
            return _anonymous;
        }

        var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

        if(expClaim is not null && long.TryParse(expClaim, out var epxUnix))
        {
            var exp = DateTimeOffset.FromUnixTimeSeconds(epxUnix);

            if (exp <= DateTimeOffset.UtcNow)
            {
                await LogoutAsync();
                return _anonymous;
            }
        }

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticatedAsync()
    {
        var authState = await GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task LogoutAsync()
    {
        await localStorage.RemoveItemAsync(TokenConstant.TokenStorageKey);
        httpClient.DefaultRequestHeaders.Authorization = null;
        MarkUserAsLoggedOut();
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}
