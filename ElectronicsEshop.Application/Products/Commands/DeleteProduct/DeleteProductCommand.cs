using MediatR;

namespace ElectronicsEshop.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommand(int id) : IRequest
{
    public int Id { get; init; } = id;
}
