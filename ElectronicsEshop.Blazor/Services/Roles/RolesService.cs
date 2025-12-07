
using ElectronicsEshop.Blazor.Utils;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Roles;

public class RolesService(HttpClient httpClient) : IRolesService
{
    public async Task<IEnumerable<string>> GetUserRolesAsync(CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync("api/admin/users/roles", ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat uživatelské role.");
            throw new ApplicationException($"Nepodařilo se získat uživatelské role, {message}");
        }

        var data = await response.Content.ReadFromJsonAsync<IEnumerable<string>>(ct);

        return data is null ? throw new ApplicationException("Nepodařilo se získat uživatelské role.") : data;
    }
    public async Task<string> GetRoleByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"api/admin/users/roles/{userId}", ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat roli uživatele.");
            throw new ApplicationException(message);
        }

        var data = await response.Content.ReadAsStringAsync(ct) ?? throw new ApplicationException("Role uživatele nebyla nalezena.");

        return data!;
    }
}
