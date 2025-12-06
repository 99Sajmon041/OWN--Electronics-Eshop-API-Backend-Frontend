using ElectronicsEshop.Blazor.Models.ApplicationUsers.CreateUser;
using ElectronicsEshop.Blazor.Models.Common;

namespace ElectronicsEshop.Blazor.Services.ApplicationUsers;

public interface IApplicationUsersService
{
    //Task<PagedResult<ApplicationUserModel>> GetAllAsync(*request dle API a Application modelu / requestu* CancellationToken ct = default);
    Task<ApplicationUserModel> GetByIdAsync(string id, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
    Task CreateUserAsync(CreateUserModel model, CancellationToken ct = default);
}
