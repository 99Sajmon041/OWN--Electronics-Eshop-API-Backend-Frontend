using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Commands.UpdateProductsStockQty;

public sealed class UpdateProductsStockQtyCommandHandler(
    IProductRepository productRepository,
    ILogger<UpdateProductsStockQtyCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductsStockQtyCommand>
{
    public async Task Handle(UpdateProductsStockQtyCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation("Začínám doskladňovat produkt s Id {ProductId} o množství {Amount}.", request.Id, request.StockQty);

        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Produkt s Id {ProductId} nebyl nalezen, nelze doskladnit.", request.Id);
            throw new NotFoundException(nameof(Product), request.Id);
        }

        await productRepository.AddStockQtyAsync(product, request.StockQty, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Produkt {ProductName} s ID {ProductId} byl doskladněn o {Amount} ks. Aktuálně skladem: {StockQty}",
            product.Name, product.Id, request.StockQty, product.StockQty);
    }
}
