namespace ElectronicsEshop.Blazor.Models.Products.Shared;

public sealed class ProductModel
{
    public int Id { get; init; }
    public string ProductCode { get; init; } = default!;
    public bool IsActive { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string CategoryName { get; init; } = default!;
    public decimal Price { get; init; }
    public decimal DiscountPercentage { get; init; }
    public decimal FinalPrice { get; init; }
    public int StockQty { get; init; }
    public string ImageUrl { get; init; } = default!;
}
