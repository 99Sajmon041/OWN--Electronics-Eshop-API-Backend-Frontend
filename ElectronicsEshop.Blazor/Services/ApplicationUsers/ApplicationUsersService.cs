using ElectronicsEshop.Blazor.Models.ApplicationUsers.CreateUser;
using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Utils;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.ApplicationUsers;

public sealed class ApplicationUsersService(HttpClient httpClient) : IApplicationUsersService
{
    public async Task<ApplicationUserModel> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"api/admin/users/{id}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat uživatele.");
            throw new KeyNotFoundException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<ApplicationUserModel>(ct) ?? throw new KeyNotFoundException("Uživatel nebyl nalezen.");

        data.OrdersCount = await httpClient.GetFromJsonAsync<int>($"api/users/{id}/orders/count", ct);

        return data;
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"api/admin/users/{id}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se smazat uživatele.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task CreateUserAsync(CreateUserModel model, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync($"api/admin/users", model, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se vytvořit uživatele.");
            throw new InvalidOperationException(message);
        }
    }
}
