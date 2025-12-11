using AutoMapper;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.Orders.DTOs;
using ElectronicsEshop.Application.Orders.Queries.Self.GetOrders;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

public sealed class GetOrdersQueryHandler(
    ILogger<GetOrdersQueryHandler> logger,
    IOrderRepository orderRepository,
    IMapper mapper,
    IUserContext userContext) : IRequestHandler<GetOrdersQuery, PagedResult<OrderListItemDto>>
{
    public async Task<PagedResult<OrderListItemDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = userContext.GetCurrentUser();

        if (currentUser is null)
        {
            logger.LogWarning("Není možné získat aktuálně přihlášeného uživatele. Nelze zpracovat požadavek na zobrazení jeho objednávek.");

            throw new UnauthorizedException("Uživatel není přihlášen.");
        }

        var (orders, ordersCount) = await orderRepository.GetPagedForCurrentUserAsync(currentUser.Id, request.Page, request.PageSize, request.OrderStatus, cancellationToken);

        var items = mapper.Map<IReadOnlyList<OrderListItemDto>>(orders);

        var result = new PagedResult<OrderListItemDto>
        {
            Items = items,
            TotalCount = ordersCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        logger.LogInformation("Uživateli {UserId} bylo vráceno {ReturnedCount} objednávek z celkového počtu {TotalCount} (stránka {Page}, PageSize {PageSize})",
            currentUser.Id, items.Count, ordersCount, request.Page, request.PageSize);

        return result;
    }
}
