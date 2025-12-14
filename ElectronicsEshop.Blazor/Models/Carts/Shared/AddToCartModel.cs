using System.ComponentModel.DataAnnotations;

namespace ElectronicsEshop.Blazor.Models.Carts.Shared;

public sealed class AddToCartModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Musíte zvolit produkt.")]
    public int ProductId { get; set; }

    [Range(1, 100, ErrorMessage = "Množství musí být v rozmezí 1 - 100.")]
    public int Quantity { get; set; }
}
