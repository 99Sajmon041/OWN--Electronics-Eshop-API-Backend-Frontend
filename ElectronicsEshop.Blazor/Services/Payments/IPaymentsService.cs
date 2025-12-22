using ElectronicsEshop.Blazor.Models.Common;
using ElectronicsEshop.Blazor.Models.Payments;

namespace ElectronicsEshop.Blazor.Services.Payments;

public interface IPaymentsService
{
    Task<PagedResult<PaymentModel>> GetAllAsync(PaymentRequest request, CancellationToken ct = default);
}
