using System.Threading;

namespace ElectronicsEshop.Application.Abstractions;

public interface IPaymentService
{
    Task<PaymentResult> CreatePayment(string userId, decimal amount, CancellationToken ct);
    Task AssignOrderAsync(int paymentId, int orderId, CancellationToken ct); // přiřadí OrderId k tabulce platby pomocí PaymentId
}