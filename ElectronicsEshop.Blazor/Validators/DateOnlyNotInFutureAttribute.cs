using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Validators;

public sealed class DateOnlyNotInFutureAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        if (value is not DateOnly date)
            return new ValidationResult("Neplatný formát data.");

        var today = DateOnly.FromDateTime(DateTime.Today);

        return date <= today
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage ?? "Datum nesmí být v budoucnosti.");
    }
}
