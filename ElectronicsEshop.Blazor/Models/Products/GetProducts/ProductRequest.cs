using ElectronicsEshop.Application.Common.Enums;

namespace ElectronicsEshop.Blazor.Models.Products.GetProducts;

public sealed class ProductRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Sort { get; set; } = "name";
    public SortOrder Order { get; set; } = SortOrder.Asc;
    public int? CategoryId { get; set; }
    public bool? IsActive { get; set; }
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }
    public int? StockMin { get; set; }
    public int? StockMax { get; set; }
    public string? Q { get; set; }
}
