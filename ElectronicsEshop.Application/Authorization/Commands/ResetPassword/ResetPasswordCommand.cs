using MediatR;

namespace ElectronicsEshop.Application.Authorization.Commands.ResetPassword;

public sealed class ResetPasswordCommand : IRequest
{
    public string UserId { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
    public string ConfirmPassword { get; init; } = string.Empty;
}
