using ElectronicsEshop.Application.Carts.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Carts.Queries.Self.GetCart;
public sealed class GetCartQuery : IRequest<CartDetailDto>
{

}