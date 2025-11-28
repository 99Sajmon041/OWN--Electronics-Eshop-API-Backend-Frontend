using ElectronicsEshop.Application.Abstractions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.Enums;
using ElectronicsEshop.Domain.RepositoryInterfaces;

namespace ElectronicsEshop.Infrastructure.Payments;

public sealed class FakePaymentService(IPaymentRepository paymentRepository) : IPaymentService
{
    public async Task AssignOrderAsync(int paymentId, int orderId, CancellationToken ct)
    {
        await paymentRepository.UpdatePaymentAsync(paymentId,orderId, DateTime.UtcNow, ct);
    }

    public async Task<PaymentResult> CreatePayment(string userId, decimal amount, CancellationToken ct)
    {
        //Validate Payment

        var payment = new Payment
        {
            UserId = userId,
            Amount = amount,
            Status = PaymentStatus.Succeeded,
            CreatedAt = DateTime.UtcNow
        };

        var paymentRecord = await paymentRepository.CreatePaymentAsync(payment, ct);

        return new PaymentResult
        {
            Success = true,
            PaymentId = paymentRecord,
            Error = null
        };
    }
}
