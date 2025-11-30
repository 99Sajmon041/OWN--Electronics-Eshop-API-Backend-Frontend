using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Auth.Register;

public sealed class RegisterModel
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

    [Required(ErrorMessage = "Heslo je povinný údaj.")]
    [MinLength(8, ErrorMessage = "Minimální délka hesla je 8 znaků.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Datum narození je povinné.")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Adresa je povinný údaj.")]
    public AddressModel Address { get; set; } = new();
}

public sealed class AddressModel
{
    [Required(ErrorMessage = "Ulice je povinný údaj.")]
    [MaxLength(100, ErrorMessage = "Maximální délka ulice je 100 znaků.")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Číslo domu je povinný údaj.")]
    [MaxLength(20, ErrorMessage = "Maximální délka čísla domu je 20 znaků.")]
    public string NumberOfHouse { get; set; } = string.Empty;

    [Required(ErrorMessage = "PSČ je povinný údaj.")]
    [MaxLength(20, ErrorMessage = "Maximální délka PSČ je 20 znaků.")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Město je povinný údaj.")]
    [MaxLength(100, ErrorMessage = "Maximální délka města je 100 znaků.")]
    public string Town { get; set; } = string.Empty;
}