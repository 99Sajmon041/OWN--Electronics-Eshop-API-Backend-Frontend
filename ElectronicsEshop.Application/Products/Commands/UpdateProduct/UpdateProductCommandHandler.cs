using AutoMapper;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(IMapper mapper,
    ILogger<UpdateProductCommandHandler> logger,
     IProductRepository productRepository,
     ICategoryRepository categoryRepository) : IRequestHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        if (!await categoryRepository.Exists(request.Data.CategoryId, cancellationToken))
            throw new NotFoundException(nameof(Category), request.Data.CategoryId);

        var other = await productRepository.GetByProductCodeAsync(request.Data.ProductCode, cancellationToken);
        if (other is not null && other.Id != request.Id)
            throw new ConflictException(request.Data.ProductCode, nameof(Product));

        mapper.Map(request.Data, entity);
        await productRepository.UpdateAsync(entity, cancellationToken);

        logger.LogInformation("Produkt {ProductId}/{ProductCode} byl upraven: {ProductName}.", entity.Id, entity.ProductCode, entity.Name);
    }
}
