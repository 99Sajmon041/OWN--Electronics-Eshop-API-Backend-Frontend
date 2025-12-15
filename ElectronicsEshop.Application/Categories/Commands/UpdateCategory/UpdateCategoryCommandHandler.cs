using AutoMapper;
using ElectronicsEshop.Application.Categories.Commands.UpdateCategory;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

public sealed class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ILogger<UpdateCategoryCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Kategorie s ID: {CategoryId} nebyla nalezena.", request.Id);
            throw new NotFoundException(nameof(Category), request.Id);
        }

        if (string.Equals(category.Name, request.Name, StringComparison.Ordinal))
        {
            logger.LogInformation("Kategorie s ID: {CategoryId} zůstává se stejným názvem: {CategoryName}.", category.Id, category.Name);

            return;
        }

        var existsWithName = await categoryRepository.ExistByNameAsync(request.Name, cancellationToken);

        if (existsWithName)
        {
            logger.LogWarning("Kategorie s názvem: {CategoryName} již existuje.", request.Name);
            throw new ConflictException( request.Name, "Kategorie", "Kategorie s tímto názvem již existuje");
        }

        category.Name = request.Name;
        await categoryRepository.UpdateAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Kategorie s ID: {CategoryId} byla změněna na nový název: {CategoryName}.", category.Id, category.Name);
    }
}
