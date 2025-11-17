using AutoMapper;
using ElectronicsEshop.Application.ApplicationUsers.DTOs;
using ElectronicsEshop.Application.Exceptions;
using ElectronicsEshop.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ElectronicsEshop.Application.ApplicationUsers.Queries.Admin.GetUser;

public sealed class GetUserQueryHandler(UserManager<ApplicationUser> userManager,
    ILogger<GetUserQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetUserQuery, ApplicationUserDto>
{
    public async Task<ApplicationUserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(ApplicationUser), request.Id);

        var userDto = mapper.Map<ApplicationUserDto>(user);

        logger.LogInformation("Admin si zobrazil uživatele: {FirstName} {LastName} (ID: {UserId})",
            user.FirstName, user.LastName, user.Id);

        return userDto;
    }
}
