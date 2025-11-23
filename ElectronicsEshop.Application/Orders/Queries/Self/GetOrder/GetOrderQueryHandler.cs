using AutoMapper;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Application.Orders.DTOs;
using ElectronicsEshop.Application.User;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Orders.Queries.Self.GetOrder;

public sealed class GetOrderQueryHandler(IOrderRepository orderRepository,
    ILogger<GetOrderQueryHandler> logger,
    IMapper mapper, IUserContext userContext) : IRequestHandler<GetOrderQuery, OrderDetailDto>
{
    public async Task<OrderDetailDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = userContext.GetCurrentUser();

        if (currentUser is null)
        {
            logger.LogWarning("Není možné získat aktuálně přihlášeného uživatele. Nelze zpracovat požadavek na zobrazení jeho objednávky.");
            throw new UnauthorizedException("Uživatel není přihlášen.");
        }

        var order = await orderRepository.GetByIdForCurrentUserAsync(request.Id, currentUser.Id, cancellationToken);
        
        if(order is null)
        {
            logger.LogWarning("Objednávka s ID: {OrderId} nebyla nalezena pro uživatele {UserId}.", request.Id, currentUser.Id);
            throw new NotFoundException(nameof(Order), request.Id);
        }

        var orderDto = mapper.Map<OrderDetailDto>(order);

        logger.LogInformation("Objednávka s ID: {OrderId} byla zobrazena uživateli s Emailem: {UserEmail}", request.Id, currentUser.Email);
        return orderDto;
    }
}
