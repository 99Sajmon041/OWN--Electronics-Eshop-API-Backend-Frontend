namespace ElectronicsEshop.Blazor.Auth.Models.Register;

public sealed class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public AddressRequest Address { get; set; } = new();
}

public sealed class AddressRequest
{
    public string Street { get; set; } = string.Empty;
    public string NumberOfHouse { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Town { get; set; } = string.Empty;
}