namespace ElectronicsEshop.Blazor.Services.Roles;

public interface IRolesService
{
    Task<IEnumerable<string>> GetUserRolesAsync(CancellationToken ct = default);
    Task<string> GetRoleByUserIdAsync(string userId, CancellationToken ct = default);
}
