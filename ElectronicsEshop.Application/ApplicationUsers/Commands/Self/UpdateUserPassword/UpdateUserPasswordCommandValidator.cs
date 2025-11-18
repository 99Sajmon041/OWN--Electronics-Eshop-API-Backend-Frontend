using FluentValidation;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Self.UpdateUserPassword;

public sealed class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(u => u.OldPassword)
            .NotEmpty().WithMessage("Aktuální heslo je povinné");

        RuleFor(u => u.NewPassword)
            .NotEmpty().WithMessage("Heslo je povinný údaj.")
            .MinimumLength(8).WithMessage("Minimální délka hesla je 8 znaků.")
            .Matches("[A-Z]").WithMessage("Heslo musí obsahovat alespoň jedno velké písmeno.")
            .Matches("[a-z]").WithMessage("Heslo musí obsahovat alespoň jedno malé písmeno.")
            .Matches("[0-9]").WithMessage("Heslo musí obsahovat alespoň jednu číslici.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Heslo musí obsahovat alespoň jeden speciální znak.");

        RuleFor(u => u.ConfirmPassword)
            .NotEmpty().WithMessage("Potvrzení hesla je povinné.")
            .Equal(u => u.NewPassword).WithMessage("Hesla se neshodují.");
    }
}
