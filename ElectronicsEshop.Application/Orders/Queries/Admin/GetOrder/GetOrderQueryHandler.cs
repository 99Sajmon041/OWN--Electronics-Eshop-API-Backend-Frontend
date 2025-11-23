using AutoMapper;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.Orders.DTOs;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Orders.Queries.Admin.GetOrder;

public sealed class GetOrderQueryHandler(IOrderRepository orderRepository,
    ILogger<GetOrderQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetOrderQuery, AdminOrderDetailDto>
{
    public async Task<AdminOrderDetailDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdForAdminAsync(request.Id, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Objednávka s ID: {OrderId} nebyla nalezena.", request.Id);
            throw new NotFoundException(nameof(Order), request.Id);
        }

        var orderDto = mapper.Map<AdminOrderDetailDto>(order);

        logger.LogInformation("Admin si zobrazil objednávku s ID: {OrderId} patřící uživateli s E-mailem: {UserEmail}", order.Id, order.ApplicationUser.Email);

        return orderDto;
    }
}
