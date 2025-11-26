using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Commands.UpdateProductImage;

public sealed class UpdateProductImageCommandHandler(
    ILogger<UpdateProductImageCommandHandler> logger,
    IProductRepository productRepository) : IRequestHandler<UpdateProductImageCommand, string>
{
    public async Task<string> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation("Byl aktualizován obrázek produktu s Id {ProductId}.", request.Id);

        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Produkt s Id {ProductId} nebyl nalezen. Nelze aktualizovat obrázek.", request.Id);
            throw new NotFoundException(nameof(Product), request.Id);
        }

        var oldImageUrl = product.ImageUrl;
        product.ImageUrl = request.ImageUrl;

        await productRepository.UpdateAsync(product, cancellationToken);

        logger.LogInformation("Obrázek produktu {ProductId} byl aktualizován.", product.Id);

        return oldImageUrl;
    }
}
