using MediatR;

namespace ElectronicsEshop.Application.Carts.Commands.RemoveCartItem;

public sealed class RemoveQtyCartItemComman : IRequest
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}
