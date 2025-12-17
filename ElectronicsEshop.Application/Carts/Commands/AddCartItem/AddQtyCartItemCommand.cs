using MediatR;

namespace ElectronicsEshop.Application.Carts.Commands.AddCartItem;

public sealed class AddQtyCartItemCommand : IRequest
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}
