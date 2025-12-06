namespace ElectronicsEshop.Blazor.Models.Common;

public class ApplicationUserModel
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool Active { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public AddressModel Address { get; set; } = default!;
    public int? OrdersCount { get; set; }
}
