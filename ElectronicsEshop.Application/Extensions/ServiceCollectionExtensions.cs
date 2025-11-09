using ElectronicsEshop.Application.User;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicsEshop.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        //services.AddMediaR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();

        return services;
    }
}
