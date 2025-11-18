using MediatR;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Self.UpdateUserPassword;

public sealed class UpdateUserPasswordCommand : IRequest
{
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
