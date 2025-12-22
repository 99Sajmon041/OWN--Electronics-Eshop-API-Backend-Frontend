using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Payments.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Payments.Queries.GetPayments;

public sealed class GetPaymentsQuery : IRequest<PagedResult<PaymentDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? UserId { get; set; }
    public int? OrderId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}
