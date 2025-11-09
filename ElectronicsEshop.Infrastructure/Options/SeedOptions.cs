namespace ElectronicsEshop.Infrastructure.Options;

public sealed class SeedOptions
{
    public string AdminEmail { get; set; } = default!;
    public string AdminPassword { get; set; } = default!;
    public string AdminRole { get; set; } = "Admin";
}
