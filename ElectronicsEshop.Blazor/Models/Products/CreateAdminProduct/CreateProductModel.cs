using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Products.CreateAdminProduct;

public sealed class CreateProductModel
{
    [Required(ErrorMessage = "Kód produktu je povinný.")]
    [MaxLength(20, ErrorMessage = "Kód produktu může mít maximálně 20 znaků.")]
    public string ProductCode { get; set; } = default!;

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Název je povinný.")]
    [MinLength(2, ErrorMessage = "Název musí mít alespoň 2 znaky.")]
    [MaxLength(100, ErrorMessage = "Název může mít maximálně 100 znaků.")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Popis je povinný.")]
    [MinLength(2, ErrorMessage = "Popis musí mít alespoň 2 znaky.")]
    [MaxLength(2000, ErrorMessage = "Popis může mít maximálně 2000 znaků.")]
    public string Description { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "Vyberte platnou kategorii.")]
    public int CategoryId { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "Cena musí být mezi 0.01 a 999999.99.")]
    public decimal Price { get; set; }

    [Range(0, 90, ErrorMessage = "Sleva musí být mezi 0 a 90 %.")]
    public decimal DiscountPercentage { get; set; }

    [Range(0, 100000, ErrorMessage = "Množství skladem musí být mezi 0 a 100000.")]
    public int StockQty { get; set; }

    [Required(ErrorMessage = "Soubor obrázku je povinný.")]
    public IBrowserFile? ImageFile { get; set; }
}
