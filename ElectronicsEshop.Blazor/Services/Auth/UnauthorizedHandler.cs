using System.Net;

namespace ElectronicsEshop.Blazor.Services.Auth;

public sealed class UnauthorizedHandler(ClientSessionService clientSessionService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var response = await base.SendAsync(request, ct);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await clientSessionService.ForceLogoutAsync("Přihlášení vypršelo. Přihlas se prosím znovu.", ct);
        }

        return response;
    }
}