using AutoMapper;
using ElectronicsEshop.Application.Categories.DTOs;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Categories.Queries.GetCategories;

public sealed class GetCategoriesQueryHandler(IMapper mapper,
    ILogger<GetCategoriesQueryHandler> logger,
    ICategoryRepository categoryRepository) : IRequestHandler<GetCategoriesQuery, PagedResult<CategoryDto>>
{
    public async Task<PagedResult<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var (items, itemsCount) = await categoryRepository.GetPagedForCategoriesAsync(request.Page, request.PageSize, cancellationToken);

        var categories = mapper.Map<IReadOnlyList<CategoryDto>>(items);

        logger.LogInformation("Admin si zobrazil seznam kategorií, celkem: {ItemsCount} položek.", itemsCount);

        return new PagedResult<CategoryDto>
        {
            Items = categories,
            TotalCount = itemsCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
