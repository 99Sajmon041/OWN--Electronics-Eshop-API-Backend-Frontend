using ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.DeactivateAccount;
using ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.UpdateAccount;
using ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.UpdatePassword;
using ElectronicsEshop.Blazor.Models.Common;

namespace ElectronicsEshop.Blazor.Services.ApplicationUsers;

public interface IApplicationUsersSelf
{
    Task<ApplicationUserModel> GetProfileAsync(CancellationToken ct = default);
    Task<RequestResult> UpdatePasswordAsync(UpdatePasswordModel model, CancellationToken ct = default);
    Task<RequestResult> DeactivateAccountAsync(DeactivateAccountModel model, CancellationToken ct = default);
    Task UpdateAccountAsync(UpdateAccountModel model, CancellationToken ct = default);
}
