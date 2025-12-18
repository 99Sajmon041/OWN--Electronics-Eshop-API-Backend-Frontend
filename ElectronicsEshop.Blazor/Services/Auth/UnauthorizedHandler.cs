using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicsEshop.Blazor.Services.Auth;

public sealed class UnauthorizedHandler(IServiceProvider services) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var response = await base.SendAsync(request, ct);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var session = services.GetRequiredService<ClientSessionService>();
            await session.ForceLogoutAsync("Přihlášení vypršelo. Přihlas se prosím znovu.", ct);
        }

        return response;
    }
}