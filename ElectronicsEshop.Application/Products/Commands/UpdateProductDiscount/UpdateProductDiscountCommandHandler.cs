using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Commands.UpdateProductDiscount;

public sealed class UpdateProductDiscountCommandHandler(
    IProductRepository productRepository,
    ILogger<UpdateProductDiscountCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductDiscountCommand>
{
    public async Task Handle(UpdateProductDiscountCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation("Začínám aktualizovat slevu produktu s Id {ProductId} na {DiscountPercentage}%.", request.Id, request.DiscountPercentage);

        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Produkt s Id {ProductId} nebyl nalezen, slevu nelze aktualizovat.", request.Id);
            throw new NotFoundException(nameof(Product), request.Id);
        }

        await productRepository.UpdateDiscountAsync(product, request.DiscountPercentage, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Sleva produktu {ProductName} (Id: {ProductId}) byla úspěšně aktualizována na {DiscountPercentage}%.",
            product.Name, product.Id, request.DiscountPercentage);
    }
}
