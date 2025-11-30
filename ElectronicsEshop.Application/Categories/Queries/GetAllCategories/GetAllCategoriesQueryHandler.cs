using AutoMapper;
using ElectronicsEshop.Application.Categories.DTOs;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler(
    ILogger<GetAllCategoriesQueryHandler> logger,
    ICategoryRepository categoryRepository,
    IMapper mapper) : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    public async Task<IReadOnlyList<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);
        var categoryDtos = mapper.Map<IReadOnlyList<CategoryDto>>(categories);

        logger.LogInformation("Admin si zobrazil seznam všech kategorií, celkem: {CategoryCount} položek.", categories.Count);

        return categoryDtos;
    }
}
