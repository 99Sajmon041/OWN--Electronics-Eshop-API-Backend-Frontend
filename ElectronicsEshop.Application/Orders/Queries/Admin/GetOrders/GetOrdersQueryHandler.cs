using AutoMapper;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Orders.DTOs;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Orders.Queries.Admin.GetOrders;

public sealed class GetOrdersQueryHandler(IOrderRepository orderRepository,
    ILogger<GetOrdersQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetOrdersQuery, PagedResult<AdminOrderListItemDto>>
{
    public async Task<PagedResult<AdminOrderListItemDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        DateOnly? from = request.From is null ? null : DateOnly.FromDateTime(request.From.Value);
        DateOnly? to = request.To is null ? null : DateOnly.FromDateTime(request.To.Value);

        var (orders, ordersCount) = await orderRepository.GetPagedForAdminAsync(request.Page, request.PageSize, request.OrderId, request.UserId, from, to, request.OrderStatus, request.CustomerEmail, cancellationToken);

        var ordersDto = mapper.Map<IReadOnlyList<AdminOrderListItemDto>>(orders);

        logger.LogInformation("Admin si zobrazil seznam objednávek, celkem: {OrdersCount}", ordersCount);

        return new PagedResult<AdminOrderListItemDto>
        {
            Items = ordersDto,
            TotalCount = ordersCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
