using AutoMapper;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler(ILogger<CreateCategoryCommandHandler> logger,
    ICategoryRepository categoryRepository,
    IMapper mapper) : IRequestHandler<CreateCategoryCommand, int>
{
    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if(await categoryRepository.ExistByNameAsync(request.Name, cancellationToken))
        {
            logger.LogWarning("Kategorie s názvem: {CategoryName} již existuje.", request.Name);
            throw new DomainException($"Kategorie s názvem {request.Name} již existuje.");
        }

        var entity = mapper.Map<Category>(request);

        var id = await categoryRepository.CreateAsync(entity, cancellationToken);
        logger.LogInformation("Admin vytvořil kategorii s ID: {CategoryId} a názvem: {CategoryName}", id, entity.Name);

        return id;
    }
}
