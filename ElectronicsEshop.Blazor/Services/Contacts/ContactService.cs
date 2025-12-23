using ElectronicsEshop.Blazor.Models.Contacts;
using ElectronicsEshop.Blazor.Utils;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Contacts;

public sealed class ContactService(HttpClient httpClient) : IContactService
{
    public async Task SendMessageToSupportAsync(ContactModel model, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/contact", model, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Zprávu se nepodařilo odeslat. Zkuste to prosím později.");
            throw new HttpRequestException(message);
        }
    }
}
