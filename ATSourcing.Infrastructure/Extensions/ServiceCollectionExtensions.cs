using ATSourcing.Infrastructure.Candidates.Extensions;
using ATSourcing.Infrastructure.Jobs.Extensions;
using ESFrame.Insfrastructure.Extensions;
using ESFrame.Insfrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ATSourcing.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddCandidateInfrastructure();
        services.AddJobInfrastructure();

        services.AddSingleton<IDomainEventModelConverter, DomainEventModelConverter>();

        services.AddLocalDomainEventDispatcher();

        return services;
    }
}
