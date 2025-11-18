using ElectronicsEshop.Application.ApplicationUsers.Shared.Validators;
using FluentValidation;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Self.UpdateUser;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("Jméno je povinný údaj.")
            .Length(2, 20).WithMessage("Jméno musí být v rozmezí 2 - 20 znaků.");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("Příjmení je povinný údaj.")
            .Length(2, 20).WithMessage("Příjmení musí být v rozmezí 2 - 20 znaků.");

        RuleFor(u => u.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now.AddYears(15)))
            .WithMessage("Datum narození nemůže být v budoucnosti a musíte být starší 15 let.");

        RuleFor(u => u.Address).SetValidator(new AddressDtoValidator());

    }
}
