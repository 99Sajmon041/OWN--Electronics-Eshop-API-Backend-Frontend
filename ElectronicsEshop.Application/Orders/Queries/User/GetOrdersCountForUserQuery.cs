using MediatR;

namespace ElectronicsEshop.Application.Orders.Queries.User;

public sealed class GetOrdersCountForUserQuery(string id) : IRequest<int>
{
    public string UserId { get; init; } = id;
}
