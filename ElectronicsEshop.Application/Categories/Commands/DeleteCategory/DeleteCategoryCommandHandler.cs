using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler(
    ILogger<DeleteCategoryCommandHandler> logger,
    ICategoryRepository categoryRepository,
    IProductRepository productRepository,
    IOrderItemRepository orderItemRepository,
    ICartItemRepository cartItemRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Kategorie s ID: {CategoryId} neexistuje.", request.Id);
            throw new NotFoundException(nameof(Category), request.Id);
        }

        var hasProducts = await productRepository.ExistsWithCategoryAsync(category.Id, cancellationToken);
        var hasOrders = await orderItemRepository.ExistsWithCategoryAsync(category.Id, cancellationToken);
        var hasCarts = await cartItemRepository.ExistsWithCategoryAsync(category.Id, cancellationToken);

        if (hasProducts || hasOrders || hasCarts)
        {
            var reasons = new List<string>();

            if (hasProducts)
                reasons.Add("produkty");
            if (hasOrders)
                reasons.Add("objednávky");
            if (hasCarts)
                reasons.Add("položky v košíku");

            var reasonText = string.Join(", ", reasons);

            logger.LogWarning("Kategorie s ID: {CategoryId} a názvem: {CategoryName} nelze smazat – existují na ni navázané {Reason}.", request.Id, category.Name, reasonText);

            throw new ConflictException(request.Id.ToString(), "Kategorie", $"Kategorie nelze smazat, protože existují navázané {reasonText}");
        }

        await categoryRepository.DeleteAsync(request.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Kategorie s ID: {CategoryId} a názvem: {CategoryName} byla smazána.", request.Id, category.Name);
    }
}
