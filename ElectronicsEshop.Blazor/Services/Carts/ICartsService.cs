using ElectronicsEshop.Blazor.Models.Carts.Shared;

namespace ElectronicsEshop.Blazor.Services.Carts;

public interface ICartsService
{
    Task<CartModel> GetCartForCurrentUserAsync(CancellationToken ct = default);
    Task DeleteItemAsync(int cartItemId, int quantity, string productName, CancellationToken ct = default);
    Task DeleteAllItemsAsync(int? itemsCount = 0, CancellationToken ct = default);
    Task AddItemAsync(ChangeQtyCartModel model, string productName, CancellationToken ct = default);
    Task RemoveItemAsync(ChangeQtyCartModel model, string productName, CancellationToken ct = default);
    Task SyncCountAsync(CancellationToken ct = default);
}
