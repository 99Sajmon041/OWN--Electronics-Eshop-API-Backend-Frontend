using System.ComponentModel.DataAnnotations;
using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Validators;

namespace ElectronicsEshop.Blazor.Models.ApplicationUsers.Self.UpdateAccount;

public sealed class UpdateAccountModel
{
    [Required(ErrorMessage = "Jméno je povinný údaj.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Jméno musí být v rozmezí 2 - 20 znaků.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Příjmení je povinný údaj.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Příjmení musí být v rozmezí 2 - 20 znaků.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Datum narození je povinné.")]
    [DateOnlyNotInFuture(ErrorMessage = "Datum narození nesmí být v budoucnosti.")]
    [MinimumAge(15, ErrorMessage = "Uživatel musí být starší než 15 let.")]
    public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddYears(-15));

    [Required(ErrorMessage = "Telefonní číslo je povinný údaj.")]
    [StringLength(20, ErrorMessage = "Maximální délka telefonního čísla je 20 znaků.")]
    [RegularExpression(@"^[0-9+\s]*$", ErrorMessage = "Telefonní číslo může obsahovat pouze číslice, mezery a znak +.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adresa je povinný údaj.")]
    public AddressModel Address { get; set; } = new();
}
