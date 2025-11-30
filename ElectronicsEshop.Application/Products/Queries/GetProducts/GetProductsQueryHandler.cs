using AutoMapper;
using ElectronicsEshop.Application.Common.Enums;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Products.DTOs;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(
    ILogger<GetProductsQueryHandler> logger,
    IProductRepository productRepository,
    IMapper mapper) : IRequestHandler<GetProductsQuery, PagedResult<ProductListItemDto>>
{
    public async Task<PagedResult<ProductListItemDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var (entities, total) = await productRepository.GetPagedAsync(
            page: request.Page,
            pageSize: request.PageSize,
            sort: request.Sort,
            asc: request.Order == SortOrder.Asc,
            q: request.Q,
            categoryId: request.CategoryId,
            isActive: request.IsActive,
            priceMin: request.PriceMin,
            priceMax: request.PriceMax,
            stockMin: request.StockMin,
            stockMax: request.StockMax,
            ct: cancellationToken);

        var items = mapper.Map<IReadOnlyList<ProductListItemDto>>(entities);

        logger.LogInformation("Získání produktů s parametry: {@Request}", request);

        return new PagedResult<ProductListItemDto>
        {
            Items = items,
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
