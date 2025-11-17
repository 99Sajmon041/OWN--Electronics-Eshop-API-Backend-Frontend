namespace ElectronicsEshop.Application.ApplicationUsers.DTOs;

public class ApplicationUserDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool Active { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public AddressDto Address { get; set; } = default!;
}
