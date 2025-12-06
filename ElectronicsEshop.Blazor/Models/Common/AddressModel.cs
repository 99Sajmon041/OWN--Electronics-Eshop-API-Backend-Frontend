using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Common;

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
