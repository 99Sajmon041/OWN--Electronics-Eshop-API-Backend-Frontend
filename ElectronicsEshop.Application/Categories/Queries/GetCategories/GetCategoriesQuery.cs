using ElectronicsEshop.Application.Categories.DTOs;
using ElectronicsEshop.Application.Common.Pagination;
using MediatR;

namespace ElectronicsEshop.Application.Categories.Queries.GetCategories;

public sealed class GetCategoriesQuery : IRequest<PagedResult<CategoryDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
