using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.ApplicationUsers.Queries.Self.GetProfile;

public sealed class GetProfileQuery : IRequest<ApplicationUserDto>
{
}
