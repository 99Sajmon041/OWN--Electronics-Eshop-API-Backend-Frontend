using ElectronicsEshop.Domain.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.Orders.Queries.User;

public sealed class GetOrdersCountForUserQueryHandler
    (ILogger<GetOrdersCountForUserQueryHandler> logger,
    IOrderRepository orderRepository) : IRequestHandler<GetOrdersCountForUserQuery, int>
{
    public async Task<int> Handle(GetOrdersCountForUserQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ordersCount = await orderRepository.GetOrdersCountForUserAsync(request.UserId, cancellationToken);

        logger.LogInformation("Byl zobrazen celkový počet objednávek uživatele s ID: {UserId}", request.UserId);

        return ordersCount;
    }
}
