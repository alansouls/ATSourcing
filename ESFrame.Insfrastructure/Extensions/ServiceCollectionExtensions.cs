using ESFrame.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ESFrame.Insfrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalDomainEventDispatcher(this IServiceCollection services)
    {
        services.TryAddScoped<IDomainEventDispatcher, LocalDomainEventDispatcher>();

        return services;
    }
}
