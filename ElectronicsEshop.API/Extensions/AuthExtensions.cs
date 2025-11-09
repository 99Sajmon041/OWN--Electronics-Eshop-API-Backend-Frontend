using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsEshop.API.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddIdentityAuth(this IServiceCollection services)
    {
        services.
            AddIdentityCore<ApplicationUser>(o =>
            {
                o.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

        services.
            AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                o.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            })
            .AddBearerToken(IdentityConstants.BearerScheme, options =>
            {
                options.BearerTokenExpiration = TimeSpan.FromHours(1);
                options.RefreshTokenExpiration = TimeSpan.FromDays(7);
            });

        services.AddAuthorization();

        return services;
    }
}
