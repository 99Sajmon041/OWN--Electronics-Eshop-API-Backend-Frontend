using AutoMapper;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Products.DTOs;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Queries.GetCatalogProducts;

public sealed class GetCatalogProductsQueryHandler
    (ILogger<GetCatalogProductsQueryHandler> logger,
    IProductRepository productRepository,
    IMapper mapper,
    IUserContext userContext) : IRequestHandler<GetCatalogProductsQuery, PagedResult<ProductListItemDto>>
{
    public async Task<PagedResult<ProductListItemDto>> Handle(GetCatalogProductsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userEmail = userContext.GetCurrentUser()?.Email ?? "Annonymous";

        var (products, totalCount) = await productRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            "price", 
            true,
            request.Q,
            request.CategoryId,
            true, null, null, 1, null, 
            cancellationToken);

        var productDtos = mapper.Map<IReadOnlyList<ProductListItemDto>>(products);

        logger.LogInformation("Zobrazení produktů uživatelem s e-mailem: {UserEmail} s parametry: {@Request}", userEmail, request);

        return new PagedResult<ProductListItemDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
