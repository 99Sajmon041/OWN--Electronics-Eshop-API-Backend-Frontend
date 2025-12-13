using ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.DeactivateAccount;
using ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.UpdateAccount;
using ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.UpdatePassword;
using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Utils;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.ApplicationUsers;

public sealed class ApplicationUsersSelf(HttpClient httpClient) : IApplicationUsersSelf
{
    public async Task<ApplicationUserModel> GetProfileAsync(CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync("api/auth/me", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat uživatelská data.");
            throw new ApplicationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<ApplicationUserModel>(ct);
            
        return data ?? throw new InvalidOperationException("Nepodařilo se získat uživatelská data.");
    }

    public async Task<RequestResult> UpdatePasswordAsync(UpdatePasswordModel model, CancellationToken ct = default)
    {
        var response = await httpClient.PatchAsJsonAsync("api/auth/me/update-password", model, ct);

        if (response.IsSuccessStatusCode)
        {
            return new RequestResult { Success = true };
        }

        var message = await response.ReadProblemMessageAsync("Nepodařilo se změnit heslo.");

        return new RequestResult
        {
            Success = false,
            ErrorMessage = message
        };
    }

    public async Task<RequestResult> DeactivateAccountAsync(DeactivateAccountModel model, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/me/deactivate", model, ct);

        if(response.IsSuccessStatusCode)
        {
            return new RequestResult { Success = true };
        }

        var message = await response.ReadProblemMessageAsync("Nepodařilo se odstranit účet.");

        return new RequestResult
        {
            Success = false,
            ErrorMessage = message
        };
    }

    public async Task UpdateAccountAsync(UpdateAccountModel model, CancellationToken ct = default)
    {
        var response = await httpClient.PatchAsJsonAsync("api/auth/me/update", model, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se upravit profil.");
            throw new HttpRequestException(message, null, response.StatusCode);
        }
    }
}
