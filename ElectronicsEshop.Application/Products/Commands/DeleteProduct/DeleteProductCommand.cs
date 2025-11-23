using MediatR;

namespace ElectronicsEshop.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommand(int id) : IRequest<string>
{
    public int Id { get; init; } = id;
}
