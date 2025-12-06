using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Products.GetProducts;
using ElectronicsEshop.Blazor.Models.Products.Shared;

namespace ElectronicsEshop.Blazor.Services.Products;

public interface IProductService
{
    Task<PagedResult<ProductModel>> GetAllAsync(ProductRequest request, CancellationToken ct = default);
    Task<ProductModel> GetByIdAsync(int productId, CancellationToken ct = default);
}
