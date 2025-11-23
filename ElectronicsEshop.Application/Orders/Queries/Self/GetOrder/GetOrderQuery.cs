using ElectronicsEshop.Application.Orders.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Orders.Queries.Self.GetOrder;

public sealed class GetOrderQuery(int id) : IRequest<OrderDetailDto>
{
    public int Id { get; init; } = id;
}
