using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.Enums;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Orders.Commands.Admin.UpdateOrderStatus;

public sealed class UpdateOrderStatusCommandHandler(ILogger<UpdateOrderStatusCommandHandler> logger,
    IOrderRepository orderRepository) : IRequestHandler<UpdateOrderStatusCommand>
{
    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        string orderState = string.Empty;
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if(order is null)
        {
            logger.LogWarning("Objednávka s ID: {OrderId} nebyla nalezena.", request.Id);
            throw new NotFoundException(nameof(Order), request.Id);
        }

        if(order.OrderStatus == OrderStatus.Cancelled || order.OrderStatus == OrderStatus.Completed)
        {
            logger.LogError("Objednávka s ID: {OrderId} se stavem Zrušena / Dokončena nelze již upravovat.", request.Id);
            throw new DomainException("Objednávka se stavem Zrušena / Dokončena nelze již upravovat.");
        }

        var newState = request.NewStatus;

        await orderRepository.UpdateOrderStatusAsync(order, newState, cancellationToken);

        switch(newState)
        {
            case OrderStatus.New :
                orderState = "Nová";
                break;

            case OrderStatus.Cancelled :
                orderState = "Zrušena";
                break;

            case OrderStatus.Shipped:
                orderState = "Odeslána";
                break;

            case OrderStatus.Completed:
                orderState = "Dokončena";
                break;
        }

        //var orderState = newState switch
        //{
        //    OrderStatus.New => "Nová",
        //    OrderStatus.Cancelled => "Zrušena",
        //    OrderStatus.Shipped => "Odeslána",
        //    OrderStatus.Completed => "Dokončena",
        //    _ => "Neznámý"
        //};


        logger.LogInformation("Objednávka s ID: {OrderId} pro uživatele s E-Mailem: {UserEmail} byla upravena na stav: {OrderStatus}",
            order.Id,
            order.ApplicationUser.Email,
            orderState);
    }
}
