namespace ElectronicsEshop.Application.ApplicationUsers.DTOs;

public class AddressDto
{
    public string Street { get; set; } = default!;
    public string NumberOfHouse { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string Town { get; set; } = default!;
}
