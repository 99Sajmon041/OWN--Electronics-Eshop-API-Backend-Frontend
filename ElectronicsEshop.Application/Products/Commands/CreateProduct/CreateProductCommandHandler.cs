using AutoMapper;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(
    ILogger<CreateProductCommandHandler> logger,
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand>
{
    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Začínám vytvářet produkt {ProductName} s kódem {ProductCode} v kategorii {CategoryId}.",
            request.Data.Name, request.Data.ProductCode, request.Data.CategoryId);

        if (!await categoryRepository.ExistsAsync(request.Data.CategoryId, cancellationToken))
        {
            logger.LogWarning("Nelze vytvořit produkt {ProductName} s kódem {ProductCode}. Kategorie s Id {CategoryId} neexistuje.",
                request.Data.Name, request.Data.ProductCode, request.Data.CategoryId);

            throw new NotFoundException(nameof(Category), request.Data.CategoryId);
        }

        if (await productRepository.ExistsByProductCodeAsync(request.Data.ProductCode, cancellationToken))
        {
            logger.LogWarning("Nelze vytvořit produkt {ProductName}. Produkt s kódem {ProductCode} již existuje.", request.Data.Name, request.Data.ProductCode);
            throw new ConflictException(request.Data.ProductCode, nameof(Product));
        }

        var product = mapper.Map<Product>(request.Data);
        await productRepository.AddAsync(product, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Produkt {ProductName} byl úspěšně zařazen.", request.Data.Name);
    }
}
