namespace ElectronicsEshop.Blazor.Models.Products.Self.GetProducts;

public sealed class ProductRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int? CategoryId { get; set; }
    public string? Q { get; set; }
}
