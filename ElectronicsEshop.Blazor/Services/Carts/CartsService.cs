using ElectronicsEshop.Blazor.Models.Carts.Shared;
using ElectronicsEshop.Blazor.UI.State;
using ElectronicsEshop.Blazor.Utils;
using System.Net.Http.Json;

namespace ElectronicsEshop.Blazor.Services.Carts;

public sealed class CartsService(HttpClient httpClient, CartState cartState) : ICartsService
{
    public async Task<CartModel> GetCartForCurrentUserAsync(CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync("api/cart", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat data z košíku.");
            throw new InvalidOperationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<CartModel>(ct);

        return data ?? throw new InvalidOperationException("Nepodařilo se získat data z košíku.");
    }

    public async Task DeleteItemAsync(int cartItemId, string productName, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"api/cart/delete/items/{cartItemId}", ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync($"Položku: {productName} se nepodařilo odebrat z košíku.");
            throw new InvalidOperationException(message);
        }

        await SetCartStateAsync(ct);
    }

    public async Task DeleteAllItemsAsync(int? itemsCount = 0, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync("api/cart/delete/items", ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync($"Nepodařilo se odstranit všechny položky (celkem: {itemsCount}).");
            throw new InvalidOperationException(message);
        }

        cartState.Clear();
    }

    public async Task AddItemAsync(ChangeQtyCartModel model, string productName, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/cart/increase/item", model, ct);

        if(!response.IsSuccessStatusCode)
        {
            var itemWord = model.Quantity == 1 ? "Položku" : "Položky";
            var message = await response.ReadProblemMessageAsync($"{itemWord} produktu: {productName} se nedaří přidat do košíku. Zkuste prosím později.");
            throw new InvalidOperationException(message);
        }

        await SetCartStateAsync(ct);
    }

    public async Task RemoveItemAsync(ChangeQtyCartModel model, string productName, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/cart/decrease/item", model, ct);

        if (!response.IsSuccessStatusCode)
        {
            var itemWord = model.Quantity == 1 ? "Položku" : "Položky";
            var message = await response.ReadProblemMessageAsync($"{itemWord} produktu: {productName} se nedaří odebrat z košíku. Zkuste prosím později.");
            throw new InvalidOperationException(message);
        }

        await SetCartStateAsync(ct);
    }

    public async Task SetCartStateAsync(CancellationToken ct = default)
    {
        var cart = await GetCartForCurrentUserAsync(ct);
        cartState.SetCount(cart.GetItemsCount());
    }
}
