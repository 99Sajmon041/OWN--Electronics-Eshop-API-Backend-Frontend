using ElectronicsEshop.Application.Abstractions;
using ElectronicsEshop.Application.User;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicsEshop.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();

        return services;
    }
}
