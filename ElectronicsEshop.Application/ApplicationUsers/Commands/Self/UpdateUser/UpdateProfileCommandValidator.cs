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

        RuleFor(u => u.PhoneNumber)
            .NotEmpty().WithMessage("Telefonní číslo je povinný údaj.")
            .MaximumLength(20).WithMessage("Maximální délka telefonního čísla je 20 znaků.")
            .Matches(@"^[0-9+\s]*$").WithMessage("Telefonní číslo může obsahovat pouze číslice, mezery a znak +.");

        RuleFor(u => u.DateOfBirth)
            .NotEmpty().WithMessage("Datum narození je povinné.")
            .Must(d => d <= DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Datum narození nesmí být v budoucnosti.")
            .Must(d => d <= DateOnly.FromDateTime(DateTime.Today.AddYears(-15)))
                .WithMessage("Uživatel musí být starší než 15 let.");

        RuleFor(u => u.Address)
            .NotNull().WithMessage("Adresa je povinný údaj.")
            .SetValidator(new AddressDtoValidator());
    }
}
