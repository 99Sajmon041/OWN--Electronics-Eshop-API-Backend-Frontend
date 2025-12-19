using ElectronicsEshop.Blazor.Models.Carts.Shared;
using ElectronicsEshop.Blazor.UI.Message;
using ElectronicsEshop.Blazor.UI.State;
using ElectronicsEshop.Blazor.Utils;
using System.Net.Http.Json;
using System.Net;

namespace ElectronicsEshop.Blazor.Services.Carts;

public sealed class CartsService(HttpClient httpClient, CartState cartState, MessageService messageService) : ICartsService
{
    public async Task<CartModel> GetCartForCurrentUserAsync(CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync("api/cart", ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se získat data z košíku.");
            throw new InvalidOperationException(message);
        }

        var data = await response.Content.ReadFromJsonAsync<CartModel>(ct);

        return data ?? throw new InvalidOperationException("Nepodařilo se získat data z košíku.");
    }

    public async Task DeleteItemAsync(int cartItemId, int quantity, string productName, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"api/cart/delete/items/{cartItemId}", ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync($"Položku: {productName} se nepodařilo odebrat z košíku.");
            await SyncCountAsync(ct);
            throw new InvalidOperationException(message);
        }

        cartState.Subtract(quantity);
    }

    public async Task DeleteAllItemsAsync(int? itemsCount = 0, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync("api/cart/delete/items", ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync($"Nepodařilo se odstranit všechny položky (celkem: {itemsCount}).");
            await SyncCountAsync(ct);
            throw new InvalidOperationException(message);
        }

        cartState.Clear();
    }

    public async Task AddItemAsync(ChangeQtyCartModel model, string productName, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/cart/increase/item", model, ct);

        if (!response.IsSuccessStatusCode)
        {
            var itemWord = model.Quantity == 1 ? "Položku" : "Položky";
            var message = await response.ReadProblemMessageAsync($"{itemWord} produktu: {productName} se nedaří přidat do košíku. Zkuste prosím později.");
            await SyncCountAsync(ct);
            throw new InvalidOperationException(message);
        }

        cartState.Add(model.Quantity);
    }

    public async Task RemoveItemAsync(ChangeQtyCartModel model, string productName, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/cart/decrease/item", model, ct);

        if (!response.IsSuccessStatusCode)
        {
            var itemWord = model.Quantity == 1 ? "Položku" : "Položky";
            var message = await response.ReadProblemMessageAsync($"{itemWord} produktu: {productName} se nedaří odebrat z košíku. Zkuste prosím později.");
            await SyncCountAsync(ct);
            throw new InvalidOperationException(message);
        }

        cartState.Subtract(model.Quantity);
    }

    public async Task SyncCountAsync(CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync("api/cart/count", ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se načíst data do ikonky košíku.");

            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                cartState.Clear();

            messageService.ShowError(message);
            return;
        }

        var count = await response.Content.ReadFromJsonAsync<int>(ct);
        cartState.SetCount(count);
    }

    public async Task SubmitOrderAsync(CancellationToken ct = default)
    {
        var response = await httpClient.PostAsync("api/cart/submit", null, ct);

        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se vytvořit objednávku.");
            throw new InvalidOperationException(message);
        }

        cartState.Clear();
    }
}
