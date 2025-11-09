namespace ElectronicsEshop.Application.User;

public sealed record CurrentUser(string Id, string Email, IEnumerable<string> Roles, DateOnly DateOfBirth)
{
    public bool IsInRole(string role) // => Roles.Contains(role);
        => Roles.Any(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));
}   
