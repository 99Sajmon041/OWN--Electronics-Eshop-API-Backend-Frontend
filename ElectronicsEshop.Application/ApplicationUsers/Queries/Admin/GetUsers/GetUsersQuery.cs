using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using ElectronicsEshop.Application.Common.Enums;
using ElectronicsEshop.Application.Common.Pagination;
using MediatR;

namespace ElectronicsEshop.Application.ApplicationUsers.Queries.Admin.GetUsers
{
    public sealed class GetUsersQuery : IRequest<PagedResult<ApplicationUserDto>>
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public string? Sort { get; init; }
        public string? Role { get; init; }
        public SortOrder Order { get; init; } = SortOrder.Asc;
    }
}
