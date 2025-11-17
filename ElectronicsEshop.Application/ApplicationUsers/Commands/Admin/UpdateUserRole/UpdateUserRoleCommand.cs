using MediatR;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.UpdateUserRole;

public sealed class UpdateUserRoleCommand : IRequest
{
    public string Id { get; set; } = default!;
    public string Role { get; set; } = default!;
}
