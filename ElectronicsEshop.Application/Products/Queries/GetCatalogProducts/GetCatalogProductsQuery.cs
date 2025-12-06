using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Application.Products.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Products.Queries.GetCatalogProducts;
public sealed class GetCatalogProductsQuery : IRequest<PagedResult<ProductListItemDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public int? CategoryId { get; init; }
    public string? Q { get; init; }
}
