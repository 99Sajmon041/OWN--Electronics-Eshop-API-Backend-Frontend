using AutoMapper;
using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using ElectronicsEshop.Application.Common.Enums;
using ElectronicsEshop.Application.Common.Pagination;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Queries.Admin.GetUsers;

public sealed class GetUsersQueryHandler(UserManager<ApplicationUser> userManager,
    ILogger<GetUsersQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetUsersQuery, PagedResult<ApplicationUserDto>>
{
    public async Task<PagedResult<ApplicationUserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = userManager.Users.AsNoTracking();

        if(!string.IsNullOrWhiteSpace(request.Role))
        {
            var usersInRole = await userManager.GetUsersInRoleAsync(request.Role);
            var ids = usersInRole.Select(u => u.Id).ToList();

            query = query.Where(u => ids.Contains(u.Id));
        }

        query = request.Order == SortOrder.Desc
            ? query.OrderByDescending(u => u.LastName)
            : query.OrderBy(u => u.LastName);

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = mapper.Map<List<ApplicationUserDto>>(users);

        logger.LogInformation("Admin zobrazil seznam uživatelů. Stránka {Page}, velikost {PageSize}, celkem {TotalCount}.",
            request.Page, request.PageSize, totalCount);

        return new PagedResult<ApplicationUserDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            Sort = null,
            Order = request.Order
        };
    }
}
