using ElectronicsEshop.Blazor.Models.ApplicationUsers.Admin.CreateUser;
using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.ApplicationUsers;

public sealed class ApplicationUsersAdminService(HttpClient httpClient) : IApplicationUsersAdminService
{
    public async Task<ApplicationUserModel> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"api/admin/users/{id}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat uživatele.");
            throw new ApplicationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<ApplicationUserModel>(ct)
            ?? throw new KeyNotFoundException("Nepodařilo se získat uživatele.");

        return data;
    }

    public async Task DeactivateAsync(string id, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"api/admin/users/{id}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se deaktivovat uživatele.");
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

    public async Task<PagedResult<ApplicationUserModel>> GetAllAsync(CommonPageRequest request, CancellationToken ct = default)
    {
        var query = new Dictionary<string, string>
        {
            ["page"] = request.Page.ToString(),
            ["pageSize"] = request.PageSize.ToString(),
            ["role"] = request.Role ?? string.Empty,
            ["email"] = request.Email ?? string.Empty
        };

        var filtered = query
            .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        var url = QueryHelpers.AddQueryString("api/admin/users", filtered);

        var response = await httpClient.GetAsync(url, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se načíst uživatele.");
            throw new ApplicationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<PagedResult<ApplicationUserModel>>(ct);

        return data ?? new PagedResult<ApplicationUserModel>
        {
            Items = [],
            TotalCount = 0,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task UpdateUserRoleAsync(string userId, string roleName, CancellationToken ct = default)
    {
        var body = new { Role = roleName };

        var response = await httpClient.PatchAsJsonAsync($"api/admin/users/{userId}", body, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se aktualizovat roli uživatele.");
            throw new InvalidOperationException(message);
        }
    }

    public async Task ActivateAsync(string id, CancellationToken ct = default)
    {
        var response = await httpClient.PatchAsync($"api/admin/users/{id}/activate", null, ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se aktivovat uživatele.");
            throw new InvalidOperationException(message);
        }
    }
}
