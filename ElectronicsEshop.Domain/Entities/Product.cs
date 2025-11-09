namespace ElectronicsEshop.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string ProductCode { get; set; } = default!;
    public bool IsActive { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Category Category { get; set; } = default!;
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; }
    public int StockQty { get; set; }
    public string ImageUrl { get; set; } = default!;
}
