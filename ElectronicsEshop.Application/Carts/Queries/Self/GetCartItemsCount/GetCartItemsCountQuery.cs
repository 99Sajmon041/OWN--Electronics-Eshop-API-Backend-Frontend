using MediatR;

namespace ElectronicsEshop.Application.Carts.Queries.Self.GetCartItemsCount;

public sealed record GetCartItemsCountQuery : IRequest<int>;

