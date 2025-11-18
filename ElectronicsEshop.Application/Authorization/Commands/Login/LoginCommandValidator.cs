using FluentValidation;

namespace ElectronicsEshop.Application.Authorization.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail je povinné pole.")
            .EmailAddress().WithMessage("zadejte platnou E-mailovou adresu.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Zadejte heslo.");
    }
}
