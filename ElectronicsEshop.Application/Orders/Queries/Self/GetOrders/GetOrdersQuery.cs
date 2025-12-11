using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Orders.DTOs;
using ElectronicsEshop.Domain.Enums;
using MediatR;

namespace ElectronicsEshop.Application.Orders.Queries.Self.GetOrders;

public sealed class GetOrdersQuery : IRequest<PagedResult<OrderListItemDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public OrderStatus? OrderStatus { get; set; }
}

