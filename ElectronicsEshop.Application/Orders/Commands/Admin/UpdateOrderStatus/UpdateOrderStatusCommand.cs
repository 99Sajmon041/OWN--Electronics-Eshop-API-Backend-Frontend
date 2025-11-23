using ElectronicsEshop.Domain.Enums;
using MediatR;

namespace ElectronicsEshop.Application.Orders.Commands.Admin.UpdateOrderStatus;

public sealed class UpdateOrderStatusCommand : IRequest
{
    public int Id { get; set; }
    public OrderStatus NewStatus { get; init; }
}
