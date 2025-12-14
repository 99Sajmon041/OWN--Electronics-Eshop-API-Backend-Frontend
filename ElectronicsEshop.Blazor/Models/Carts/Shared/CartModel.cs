namespace ElectronicsEshop.Blazor.Models.Carts.Shared;

public sealed class CartModel
{
    public string UserId { get; set; } = default!;
    public string UserEmail { get; set; } = default!;
    public int TotalQuantity { get; set; }
    public decimal TotalPrice { get; set; }
    public IReadOnlyList<CartItemModel> Items { get; set; } = [];
}
