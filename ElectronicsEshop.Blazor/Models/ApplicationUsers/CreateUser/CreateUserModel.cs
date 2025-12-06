using System.ComponentModel.DataAnnotations;
using ElectronicsEshop.Blazor.Models.Common;

namespace ElectronicsEshop.Blazor.Models.ApplicationUsers.CreateUser;

public sealed class CreateUserModel
{
    [Required(ErrorMessage = "Jméno je povinný údaj.")]
    [MaxLength(100, ErrorMessage = "Maximální délka jména je 100 znaků.")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "Příjmení je povinný údaj.")]
    [MaxLength(100, ErrorMessage = "Maximální délka příjmení je 100 znaků.")]
    public string LastName { get; set; } = default!;

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
    public AddressModel Address { get; set; } = default!;
}
