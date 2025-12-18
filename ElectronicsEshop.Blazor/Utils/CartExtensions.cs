using ElectronicsEshop.Blazor.Models.Carts.Shared;

namespace ElectronicsEshop.Blazor.Utils;

public static class CartExtensions
{
    public static int GetItemsCount(this CartModel cart)
    {
        return cart.Items?.Sum(ci => ci.Quantity) ?? 0;
    }
}
