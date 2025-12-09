namespace ElectronicsEshop.Application.Carts.DTOs;

public sealed class CartDetailDto
{
    public string UserId { get; set; } = default!; 
    public string UserEmail { get; set; } = default!;
    public int TotalQuantity { get; set; }
    public decimal TotalPrice { get; set; }
    public IReadOnlyList<CartItemDto> Items { get; set; } = [];
}
