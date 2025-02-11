using ATSourcing.Application.Jobs.Interfaces;
using ESFrame.Insfrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ATSourcing.Infrastructure.Jobs.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddSingleton<IDomainEventConverterModule, DomainEventConverterModule>();

        return services;
    }
}
