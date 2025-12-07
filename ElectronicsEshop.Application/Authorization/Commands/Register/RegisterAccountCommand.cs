using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Authorization.Commands.Register;

public sealed class RegisterAccountCommand : IRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string UserName => Email;
    public string Password { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public AddressDto Address { get; set; } = default!;
}
