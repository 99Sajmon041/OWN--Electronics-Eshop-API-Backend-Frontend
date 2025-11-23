using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler(
    ILogger<DeleteProductCommandHandler> logger,
    IProductRepository productRepository,
    IOrderItemRepository orderItemRepository,
    ICartItemRepository cartItemRepository) : IRequestHandler<DeleteProductCommand, string>
{
    public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation("Začínám mazat produkt s Id {ProductId}.", request.Id);

        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Produkt s Id {ProductId} nebyl nalezen. Mazání nelze provést.", request.Id);
            throw new NotFoundException(nameof(Product), request.Id);
        }

        if (await orderItemRepository.ExistsForProductAsync(product.Id, cancellationToken))
        {
            logger.LogWarning("Produkt {ProductId} ({ProductName}) nelze smazat – je použit v objednávkách.", product.Id, product.Name);

            throw new DomainException("Produkt nelze smazat – je použit v objednávkách.");
        }

        if (await cartItemRepository.ExistsForProductAsync(product.Id, cancellationToken))
        {
            logger.LogWarning("Produkt {ProductId} ({ProductName}) nelze smazat – mají ho uživatelé v košíku.", product.Id, product.Name);
            throw new DomainException("Produkt nelze smazat – mají ho uživatelé v košíku.");
        }

        var imageUrl = product.ImageUrl;

        await productRepository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Produkt {ProductId} ({ProductName}) byl odstraněn.", product.Id, product.Name);

        return imageUrl;
    }
}
