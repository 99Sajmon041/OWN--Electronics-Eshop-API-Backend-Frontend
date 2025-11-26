using MediatR;

namespace ElectronicsEshop.Application.Carts.Commands.DeleteCartItem;

public sealed class DeleteCartItemCommand(int id) : IRequest
{
    public int Id { get; init; } = id;
}
