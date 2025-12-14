using ElectronicsEshop.Blazor.Models.Carts.Shared;
using ElectronicsEshop.Blazor.Models.Common;

namespace ElectronicsEshop.Blazor.Services.Carts
{
    public interface ICartsAdminService
    {
        Task<PagedResult<CartModel>> GetAllAsync(CommonPageRequest request, CancellationToken ct = default);
    }
}
