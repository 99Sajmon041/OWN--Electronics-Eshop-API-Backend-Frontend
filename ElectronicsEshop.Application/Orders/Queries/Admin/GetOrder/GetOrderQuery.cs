using ElectronicsEshop.Application.Orders.DTOs;
using MediatR;

namespace ElectronicsEshop.Application.Orders.Queries.Admin.GetOrder;

public sealed class GetOrderQuery(int id) : IRequest<AdminOrderDetailDto>
{
    public int Id { get; init; } = id;
}
