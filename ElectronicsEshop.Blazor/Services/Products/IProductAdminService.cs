using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Products.Admin.CreateAdminProduct;
using ElectronicsEshop.Blazor.Models.Products.Admin.GetAdminProducts;
using ElectronicsEshop.Blazor.Models.Products.Admin.UpdateAdminProduct;
using ElectronicsEshop.Blazor.Models.Products.Shared;
using Microsoft.AspNetCore.Components.Forms;

namespace ElectronicsEshop.Blazor.Services.Products;

public interface IProductAdminService
{
    Task<PagedResult<ProductModel>> GetAllAsync(ProductRequest request, CancellationToken ct = default);
    Task<ProductModel> GetByIdAsync(int productId, CancellationToken ct = default);
    Task CreateAsync(CreateProductModel model, CancellationToken ct = default);
    Task UpdateStockQtyAsync(int productId, int newStockQty, CancellationToken ct = default);
    Task SetActiveAsync(int productId, bool isActive, CancellationToken ct = default);
    Task UpdateDiscountAsync(int productId, decimal newDiscount, CancellationToken ct = default);
    Task UpdateImageAsync(int productId, IBrowserFile file, CancellationToken ct = default);
    Task DeleteAsync(int productId, CancellationToken ct = default);
    Task UpdateAsync(int productId, UpdateProductModel model, CancellationToken ct = default);
}
