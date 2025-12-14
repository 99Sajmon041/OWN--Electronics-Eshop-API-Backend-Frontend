using ElectronicsEshop.Blazor.Models.Products.Admin.UpdateAdminProduct;
using ElectronicsEshop.Blazor.Models.Products.Shared;

namespace ElectronicsEshop.Blazor.Models.Mappers;

public static class ProductMapper
{
    public static UpdateProductModel ToUpdateModel(this ProductModel source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new UpdateProductModel
        {
            ProductCode = source.ProductCode,
            IsActive = source.IsActive,
            Name = source.Name,
            Description = source.Description,
            CategoryId = 0,
            Price = source.Price,
            DiscountPercentage = source.DiscountPercentage,
            StockQty = source.StockQty,
            ImageUrl = source.ImageUrl
        };
    }
}
