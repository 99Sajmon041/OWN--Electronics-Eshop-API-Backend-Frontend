using MediatR;

namespace ElectronicsEshop.Application.Carts.Commands.AddCartItem;

public sealed class AddCartItemCommand : IRequest
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}
