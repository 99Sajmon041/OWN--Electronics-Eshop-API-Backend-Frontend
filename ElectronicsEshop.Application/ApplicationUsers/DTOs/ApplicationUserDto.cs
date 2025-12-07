namespace ElectronicsEshop.Application.ApplicationUsers.DTOs;

public class ApplicationUserDto
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool Active { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public AddressDto Address { get; set; } = default!;
}
