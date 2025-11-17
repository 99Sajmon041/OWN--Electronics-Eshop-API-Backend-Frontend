using MediatR;

namespace ElectronicsEshop.Application.Account.Commands.ForgotPassword;

public sealed class ForgotPasswordCommand : IRequest
{
    public string Email { get; set; } = default!;
}
