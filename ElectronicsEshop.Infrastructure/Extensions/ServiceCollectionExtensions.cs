using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Domain.RepositoryInterfaces;
using ElectronicsEshop.Infrastructure.Database;
using ElectronicsEshop.Infrastructure.Options;
using ElectronicsEshop.Infrastructure.Repositories;
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
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredUniqueChars = 1;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, EshopUserClaimsPrincipalFactory>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
        })
        .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorization();

        services.AddScoped<IDefaultDataSeeder, DefaultDataSeeder>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<ICartRepository, CartRepository>();

        return services;
    }
}
