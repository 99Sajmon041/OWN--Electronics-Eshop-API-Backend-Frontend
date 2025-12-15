using MediatR;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.ReActivateuser;

public sealed record ReActivateUserCommand(string UserId) : IRequest;
