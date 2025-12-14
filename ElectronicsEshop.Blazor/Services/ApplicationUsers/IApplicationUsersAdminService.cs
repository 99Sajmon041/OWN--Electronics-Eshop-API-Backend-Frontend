using ElectronicsEshop.Blazor.Models.ApplicationUsers.Admin.CreateUser;
using ElectronicsEshop.Blazor.Models.Common;

namespace ElectronicsEshop.Blazor.Services.ApplicationUsers;

public interface IApplicationUsersAdminService
{
    Task<PagedResult<ApplicationUserModel>> GetAllAsync(CommonPageRequest request, CancellationToken ct = default);
    Task<ApplicationUserModel> GetByIdAsync(string id, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
    Task CreateUserAsync(CreateUserModel model, CancellationToken ct = default);
    Task UpdateUserRoleAsync(string userId, string roleName, CancellationToken ct = default);
}
