using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using FluentValidation;

namespace ElectronicsEshop.Application.ApplicationUsers.Shared.Validators;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(a => a.Street)
            .NotEmpty().WithMessage("Ulice je povinný údaj.")
            .MaximumLength(100).WithMessage("Maximální délka je 100 znaků.");

        RuleFor(a => a.NumberOfHouse)
            .NotEmpty().WithMessage("Číslo domu je povinný údaj.")
            .MaximumLength(20).WithMessage("Maximální délka je 20 znaků.");

        RuleFor(a => a.PostalCode)
            .NotEmpty().WithMessage("PSČ je povinný údaj.")
            .MaximumLength(20).WithMessage("Maximální délka je 20 znaků.");

        RuleFor(a => a.Town)
            .NotEmpty().WithMessage("Město je povinný údaj.")
            .MaximumLength(100).WithMessage("Maximální délka je 100 znaků.");
    }
}
