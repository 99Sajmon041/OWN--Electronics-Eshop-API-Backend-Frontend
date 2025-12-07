using MediatR;

namespace ElectronicsEshop.Application.Role.Queries.GetRole;

public sealed class GetRoleQuery(string id) : IRequest<string>
{
    public string Id { get; init; } = id;
}
