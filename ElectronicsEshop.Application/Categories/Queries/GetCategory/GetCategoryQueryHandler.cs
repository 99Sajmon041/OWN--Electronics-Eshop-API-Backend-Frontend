using AutoMapper;
using ElectronicsEshop.Application.Categories.DTOs;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Categories.Queries.GetCategory;

public sealed class GetCategoryQueryHandler(ILogger<GetCategoryQueryHandler> logger,
    IMapper mapper, ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Nebyla nalezena kategorie s ID: {CategoryId}", request.Id);
            throw new NotFoundException(nameof(Category), request.Id);
        }

        var categoryDto = mapper.Map<CategoryDto>(category);

        logger.LogInformation("Admin si zobrazil kategorii s ID: {CategoryId} a názvem: {CategoryName}", category.Id, category.Name);

        return categoryDto;
    }
}
