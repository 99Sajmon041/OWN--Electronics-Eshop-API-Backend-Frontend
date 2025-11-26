using AutoMapper;
using ElectronicsEshop.Application.Carts.DTOs;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Carts.Queries.Admin.GetCarts;

public sealed class GetCartsQueryHandler(ILogger<GetCartsQueryHandler> logger,
    ICartRepository cartRepository,
    IMapper mapper) : IRequestHandler<GetCartsQuery, PagedResult<CartDetailDto>>
{
    public async Task<PagedResult<CartDetailDto>> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var (carts, cartsCount) = await cartRepository.GetAllCartsForAdminAsync(request.UserId, request.Page, request.PageSize, cancellationToken);

        logger.LogInformation("Admin si zobrazil seznam uživatelských košíků, celkem: {CartsCount}", cartsCount);

        var items = mapper.Map<List<CartDetailDto>>(carts);

        return new PagedResult<CartDetailDto>
        {
            Items = items,
            TotalCount = cartsCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
