using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Orders.DTOs;
using ElectronicsEshop.Domain.Enums;
using MediatR;

namespace ElectronicsEshop.Application.Orders.Queries.Admin.GetOrders;

public sealed class GetOrdersQuery : IRequest<PagedResult<AdminOrderListItemDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public OrderStatus? OrderStatus { get; init; }
    public string? CustomerEmail { get; init; }
    public int? OrderId { get; init; }
    public string? UserId { get; init; }
}
