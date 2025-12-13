using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Validators;

public sealed class MinimumAgeAttribute(int minAge) : ValidationAttribute
{
    public int MinAge { get; } = minAge;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        if (value is not DateOnly dob)
            return new ValidationResult("Neplatný formát data.");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var limit = today.AddYears(-MinAge);

        return dob <= limit
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage ?? $"Uživatel musí být starší než {MinAge} let.");
    }
}
