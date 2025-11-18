using FluentValidation;

namespace ElectronicsEshop.Application.Authorization.Commands.ResetPassword;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId je povinný údaj.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token je povinný údaj.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Heslo je povinný údaj.")
            .MinimumLength(8).WithMessage("Minimální délka hesla je 8 znaků.")
            .Matches("[A-Z]").WithMessage("Heslo musí obsahovat alespoň jedno velké písmeno.")
            .Matches("[a-z]").WithMessage("Heslo musí obsahovat alespoň jedno malé písmeno.")
            .Matches("[0-9]").WithMessage("Heslo musí obsahovat alespoň jednu číslici.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Heslo musí obsahovat alespoň jeden speciální znak.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Potvrzení hesla je povinný údaj.")
            .Equal(x => x.NewPassword).WithMessage("Hesla se musí shodovat.");
    }
}
