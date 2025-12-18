using ElectronicsEshop.Blazor.Services.Auth;
using ElectronicsEshop.Blazor.UI.Message;
using ElectronicsEshop.Blazor.UI.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

public sealed class ClientSessionService(
    CartState cartState,
    MessageService messageService,
    AuthenticationStateProvider authStateProvider,
    NavigationManager nav)
{
    private int handled;

    public async Task ForceLogoutAsync(string message, CancellationToken ct = default)
    {
        if (Interlocked.Exchange(ref handled, 1) == 1)
            return;

        cartState.Clear();

        if (authStateProvider is ApiAuthenticationStateProvider apiAuth)
            await apiAuth.LogoutAsync();

        messageService.ShowInfo(message);
        nav.NavigateTo("/login");
    }
}