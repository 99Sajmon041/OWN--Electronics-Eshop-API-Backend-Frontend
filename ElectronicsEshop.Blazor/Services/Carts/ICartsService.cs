using ElectronicsEshop.Blazor.Models.Carts.Shared;

namespace ElectronicsEshop.Blazor.Services.Carts;

public interface ICartsService
{
    Task<CartModel> GetCartForCurrentUserAsync(CancellationToken ct = default);
    Task DeleteItemAsync(int cartItemId, string productName, CancellationToken ct = default);
    Task DeleteAllItemSAsync(int itemsCount, CancellationToken ct = default);
    Task AddItemAsync(ChangeQtyCartModel model, string productName, CancellationToken ct = default);
    Task RemoveItemAsync(ChangeQtyCartModel model, string productName, CancellationToken ct = default);

}
