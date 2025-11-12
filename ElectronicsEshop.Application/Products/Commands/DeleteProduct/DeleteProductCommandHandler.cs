using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger,
    IProductRepository productRepository) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        //ověřit v OrderRepository / OrderItemRepository, že neexistuje záznam, který je vázaný prodle ProductId v tabulce OrderItem, protože tam mám Restrict mazání !

        await productRepository.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Produkt {ProductId} ({ProductName}) byl odstraněn.", product.Id, product.Name);
    }
}
