using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Self.UpdateUser;

public sealed class UpdateProfileCommand : IRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public AddressDto Address { get; set; } = default!;
}
