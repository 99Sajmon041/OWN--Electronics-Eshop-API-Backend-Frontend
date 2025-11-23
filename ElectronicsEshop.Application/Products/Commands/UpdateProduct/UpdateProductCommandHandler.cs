using AutoMapper;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(
    IMapper mapper,
    ILogger<UpdateProductCommandHandler> logger,
    IProductRepository productRepository,
    ICategoryRepository categoryRepository) : IRequestHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation("Začínám aktualizovat produkt s Id {ProductId}.", request.Id);

        var entity = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            logger.LogWarning("Produkt s Id {ProductId} nebyl nalezen, aktualizaci nelze provést.", request.Id);
            throw new NotFoundException(nameof(Product), request.Id);
        }

        if (!await categoryRepository.ExistsAsync(request.Data.CategoryId, cancellationToken))
        {
            logger.LogWarning("Nelze aktualizovat produkt {ProductId} ({ProductCode}). Kategorie s Id {CategoryId} neexistuje.",
                entity.Id, request.Data.ProductCode, request.Data.CategoryId);

            throw new NotFoundException(nameof(Category), request.Data.CategoryId);
        }

        var other = await productRepository.GetByProductCodeAsync(request.Data.ProductCode, cancellationToken);
        if (other is not null && other.Id != request.Id)
        {
            logger.LogWarning("Nelze aktualizovat produkt {ProductId}. Produkt s kódem {ProductCode} již existuje pod Id {ExistingProductId}.",
                request.Id, request.Data.ProductCode, other.Id);

            throw new ConflictException(request.Data.ProductCode, nameof(Product));
        }

        var originalImageUrl = entity.ImageUrl;

        mapper.Map(request.Data, entity);

        entity.ImageUrl = originalImageUrl;

        await productRepository.UpdateAsync(entity, cancellationToken);

        logger.LogInformation("Produkt {ProductId}/{ProductCode} byl úspěšně upraven: {ProductName}.", entity.Id, entity.ProductCode, entity.Name);
    }
}
