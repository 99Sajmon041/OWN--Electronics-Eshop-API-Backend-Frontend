using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Products.CreateProduct;
using ElectronicsEshop.Blazor.Models.Products.GetProducts;

namespace ElectronicsEshop.Blazor.Services.Products;

public interface IProductAdminService
{
    Task<PagedResult<ProductListItemModel>> GetAllAsync(ProductRequest request, CancellationToken ct = default);
    Task CreateAsync(CreateProductRequest request, CancellationToken ct = default);
}
