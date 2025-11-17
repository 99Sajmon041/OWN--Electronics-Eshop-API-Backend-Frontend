using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.ApplicationUsers.Queries.Admin.GetUser;

public sealed class GetUserQuery(string id) : IRequest<ApplicationUserDto>
{
    public string Id { get; init; } = id;
}
