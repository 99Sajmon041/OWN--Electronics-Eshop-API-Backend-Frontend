using MediatR;

namespace ElectronicsEshop.Application.Authorization.Commands.Login;

public sealed class LoginCommand : IRequest<bool>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; }
}
