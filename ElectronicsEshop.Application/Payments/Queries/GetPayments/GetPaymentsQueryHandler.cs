using AutoMapper;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Payments.DTOs;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Payments.Queries.GetPayments;

public sealed class GetPaymentsQueryHandler(
    ILogger<GetPaymentsQueryHandler> logger,
    IPaymentRepository paymentRepository,
    IMapper mapper) : IRequestHandler<GetPaymentsQuery, PagedResult<PaymentDto>>
{
    public async Task<PagedResult<PaymentDto>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var (payments, paymentsCount) = await paymentRepository.GetPagedAsync(
            request.Page, 
            request.PageSize,
            request.UserId, 
            request.OrderId, 
            request.From,
            request.To,
            cancellationToken);

        var items = mapper.Map<IReadOnlyList<PaymentDto>>(payments);

        logger.LogInformation(
            "Admin si zobrazil přehled plateb. Strana: {Page}, PageSize: {PageSize}, TotalCount: {ItemsCount}, UserId: {UserId}, OrderId: {OrderId}, From: {From}, To: {To}",
            request.Page,
            request.PageSize,
            paymentsCount,
            request.UserId,
            request.OrderId,
            request.From,
            request.To);

        return new PagedResult<PaymentDto>
        { 
            Items = items,
            TotalCount = paymentsCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
