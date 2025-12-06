using ElectronicsEshop.Blazor.Models.Products.Shared;
using ElectronicsEshop.Blazor.Models.Products.UpdateAdminProduct;

namespace ElectronicsEshop.Blazor.Models.Mappers;

public static class ProductMapper
{
    public static UpdateProductModel ToUpdatemodel(this ProductModel source)
    {
        if(source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

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
