using Blazored.LocalStorage;
using ElectronicsEshop.Blazor.Models.Constants;
using ElectronicsEshop.Blazor.Services.Carts;
using ElectronicsEshop.Blazor.UI.State;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ElectronicsEshop.Blazor.Services.Auth;

public sealed class ApiAuthenticationStateProvider(
    ILocalStorageService localStorage,
    HttpClient httpClient,
    TokenExpiryService tokenExpiry,
    CartState cartState,
    IServiceProvider services
) : AuthenticationStateProvider
{

    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly AuthenticationState _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>(TokenConstant.TokenStorageKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            tokenExpiry.Cancel();
            httpClient.DefaultRequestHeaders.Authorization = null;
            return _anonymous;
        }

        JwtSecurityToken jwtToken;

        try
        {
            jwtToken = _tokenHandler.ReadJwtToken(token);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);

            tokenExpiry.Cancel();
            httpClient.DefaultRequestHeaders.Authorization = null;
            await LogoutAsync();
            return _anonymous;
        }

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var expValue = jwtToken.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)
            ?.Value;

        if (expValue is not null && long.TryParse(expValue, out var expUnix))
        {
            var expiresAtUtc = DateTimeOffset.FromUnixTimeSeconds(expUnix);

            if (expiresAtUtc <= DateTimeOffset.UtcNow)
            {
                tokenExpiry.Cancel();
                await LogoutAsync();
                return _anonymous;
            }

            tokenExpiry.Schedule(
                jwtToken: jwtToken,
                onPreExpiryAsync: async () =>
                {
                    try
                    {
                        var cartsService = services.GetRequiredService<ICartsService>();
                        await cartsService.DeleteAllItemsAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                },
                onExpiredAsync: async () =>
                {
                    try
                    {
                        await LogoutAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                },
                preExpiryOffset: TimeSpan.FromSeconds(60)
            );
        }
        else
        {
            tokenExpiry.Cancel();
        }

        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
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
        tokenExpiry.Cancel();
        cartState.Clear();

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
