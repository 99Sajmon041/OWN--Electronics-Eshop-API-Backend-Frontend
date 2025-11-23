using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Orders.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Orders.Queries.Self.GetOrders;

public sealed class GetOrdersQuery : IRequest<PagedResult<OrderListItemDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}

