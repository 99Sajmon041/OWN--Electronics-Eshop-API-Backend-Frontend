using MediatR;

namespace ElectronicsEshop.Application.Authorization.Commands.ForgotPassword;

public sealed class ForgotPasswordCommand : IRequest
{
    public string Email { get; set; } = default!;
}
