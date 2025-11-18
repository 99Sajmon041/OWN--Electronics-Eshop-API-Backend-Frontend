using FluentValidation;

namespace ElectronicsEshop.Application.Authorization.Commands.ForgotPassword;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("E-mail je povinný údaj.")
            .EmailAddress().WithMessage("Zadejte e-mail ve správném formátu.");
    }
}
