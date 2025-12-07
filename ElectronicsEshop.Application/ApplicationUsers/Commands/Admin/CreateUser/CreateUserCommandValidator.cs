using ElectronicsEshop.Application.ApplicationUsers.Shared.Validators;
using FluentValidation;

namespace ElectronicsEshop.Application.ApplicationUsers.Commands.Admin.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("Jméno je povinný údaj.")
            .Length(2, 20).WithMessage("jméno musí být v rozmezí 2 - 20 znaků.");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("Příjmení je povinný údaj.")
            .Length(2, 20).WithMessage("Příjmení musí být v rozmezí 2 - 20 znaků.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("E-Mail je povinný údaj.")
            .EmailAddress().WithMessage("Zadejte E-Mail ve správném formátu.");

        RuleFor(u => u.PhoneNumber)
         .NotEmpty().WithMessage("Telefonní číslo je povinný údaj.")
         .MaximumLength(20).WithMessage("Maximální délka telefonního čísla je 20 znaků.")
         .Matches(@"^[0-9+\s]*$").WithMessage("Telefonní číslo může obsahovat pouze číslice, mezery a znak +.");


        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Heslo je povinný údaj.")
            .MinimumLength(8).WithMessage("Minimální délka hesla je 8 znaků.")
            .Matches("[A-Z]").WithMessage("Heslo musí obsahovat alespoň jedno velké písmeno.")
            .Matches("[a-z]").WithMessage("Heslo musí obsahovat alespoň jedno malé písmeno.")
            .Matches("[0-9]").WithMessage("Heslo musí obsahovat alespoň jednu číslici.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Heslo musí obsahovat alespoň jeden speciální znak.");

        RuleFor(u => u.Role)
            .NotEmpty().WithMessage("Role je povinný údaj.");

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
