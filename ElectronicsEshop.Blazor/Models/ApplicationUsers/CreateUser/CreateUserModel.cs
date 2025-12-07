using System.ComponentModel.DataAnnotations;
using ElectronicsEshop.Blazor.Models.Common;

namespace ElectronicsEshop.Blazor.Models.ApplicationUsers.CreateUser;

public sealed class CreateUserModel
{
    [Required(ErrorMessage = "Jméno je povinný údaj.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Délka jména musí být mezi 2 a 100 znaky.")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "Příjmení je povinný údaj.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Délka příjmení musí být mezi 2 a 100 znaky.")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "Telefonní číslo je povinný údaj.")]
    [MaxLength(20, ErrorMessage = "Maximální délka telefonního čísla je 20 znaků.")]
    [RegularExpression(@"^[0-9+\s]*$", ErrorMessage = "Telefonní číslo může obsahovat pouze číslice, mezery a znak +.")]
    public string PhoneNumber { get; set; } = default!;

    [Required(ErrorMessage = "Email je povinný údaj.")]
    [EmailAddress(ErrorMessage = "Zadejte platnou emailovou adresu.")]
    [MaxLength(256, ErrorMessage = "Maximální délka emailu je 256 znaků.")]
    public string Email { get; set; } = default!;
    public string UserName => Email;

    [Required(ErrorMessage = "Heslo je povinný údaj.")]
    [MinLength(8, ErrorMessage = "Heslo musí mít alespoň 8 znaků.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Role je povinný údaj.")]
    public string Role { get; set; } = default!;

    [Required(ErrorMessage = "Datum narození je povinný údaj.")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Adresa je povinný údaj.")]
    public AddressModel Address { get; set; } = new();
}
