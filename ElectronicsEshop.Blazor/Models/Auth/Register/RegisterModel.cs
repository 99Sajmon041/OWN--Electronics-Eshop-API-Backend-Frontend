using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Validators;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Auth.Register;

public sealed class RegisterModel : IHasAddress, IValidatableObject
{
    [Required(ErrorMessage = "Jméno je povinný údaj.")]
    [MinLength(2, ErrorMessage = "Jméno musí mít alespoň 2 znaky.")]
    [MaxLength(20, ErrorMessage = "Jméno může mít maximálně 20 znaků.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Příjmení je povinný údaj.")]
    [MinLength(2, ErrorMessage = "Příjmení musí mít alespoň 2 znaky.")]
    [MaxLength(20, ErrorMessage = "Příjmení může mít maximálně 20 znaků.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail je povinný údaj.")]
    [EmailAddress(ErrorMessage = "Zadejte e-mail ve správném formátu.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefonní číslo je povinný údaj.")]
    [MaxLength(20, ErrorMessage = "Maximální délka telefonního čísla je 20 znaků.")]
    [RegularExpression(@"^[0-9+\s]*$", ErrorMessage = "Telefonní číslo může obsahovat pouze číslice, mezery a znak +.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Heslo je povinný údaj.")]
    [MinLength(8, ErrorMessage = "Minimální délka hesla je 8 znaků.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Datum narození je povinné.")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Adresa je povinný údaj.")]
    public AddressModel Address { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        results.AddRange(this.ValidateAddress());
        return results;
    }
}
