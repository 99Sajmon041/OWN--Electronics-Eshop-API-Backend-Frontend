using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Products.Commands.SetProductState;

public sealed class SetProductStateCommandHandler(
    IProductRepository productRepository,
    ILogger<SetProductStateCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<SetProductStateCommand>
{
    public async Task Handle(SetProductStateCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation("Začínám měnit stav produktu s Id {ProductId} na IsActive = {IsActive}.", request.Id,  request.IsActive);

        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Produkt s Id {ProductId} nebyl nalezen, stav nelze změnit.", request.Id);

            throw new NotFoundException(nameof(Product), request.Id);
        }

        await productRepository.SetStateOfProductAsync(product, request.IsActive, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string stateOfProduct = request.IsActive ? "aktivní" : "neaktivní";

        logger.LogInformation("Produktu {ProductName} (Id: {ProductId}) byl úspěšně nastaven stav {StateOfProduct}.", product.Name, product.Id, stateOfProduct);
    }
}
