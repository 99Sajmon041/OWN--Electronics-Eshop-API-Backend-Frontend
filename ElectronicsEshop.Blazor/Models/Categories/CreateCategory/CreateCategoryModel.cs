using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Categories.CreateCategory;

public sealed class CreateCategoryModel
{
    [Required(ErrorMessage = "Název kategorie je povinný.")]
    [StringLength(200, MinimumLength = 2,  ErrorMessage = "Název kategorie musí být v rozmezí 2–200 znaků.")]
    public string Name { get; set; } = default!;
}