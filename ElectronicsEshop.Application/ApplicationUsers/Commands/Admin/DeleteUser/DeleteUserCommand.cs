using MediatR;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.DeleteUser;

public sealed class DeleteUserCommand(string id) : IRequest
{
    public string Id { get; init; } = id;
}
