// Removed legacy Ordering Application Services
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitectureDemo.Modules.Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingApplication(this IServiceCollection services)
    {
        // Register MediatR for this assembly explicitly, 
        // to handle local command/queries if any.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Note: CQRS Handlers are auto-registered via MediatR above

        return services;
    }
}
