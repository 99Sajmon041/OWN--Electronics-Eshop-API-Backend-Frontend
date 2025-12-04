using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Categories.UpdateCategory;

public sealed class UpdateCategoryModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Název kategorie je povinný.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Název kategorie musí být v rozmezí 2 - 100 znaků.")]
    public string Name { get; set; } = default!;
}