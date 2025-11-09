using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Infrastructure.Database;
using ElectronicsEshop.Infrastructure.Options;
using ElectronicsEshop.Infrastructure.Security;
using ElectronicsEshop.Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicsEshop.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddOptions<SeedOptions>()
            .Bind(configuration.GetSection("Seed"));

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders()
        .AddApiEndpoints();

        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, EshopUserClaimsPrincipalFactory>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
        })
        .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorization();

        services.AddScoped<IDefaultDataSeeder, DefaultDataSeeder>();

        return services;
    }
}
