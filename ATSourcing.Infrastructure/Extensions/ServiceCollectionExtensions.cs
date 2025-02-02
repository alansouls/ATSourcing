using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Infrastructure.Candidates;
using ATSourcing.Infrastructure.Candidates.Extensions;
using ESFrame.Insfrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ATSourcing.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddCandidateInfrastructure();

        services.AddSingleton<IDomainEventModelConverter, DomainEventModelConverter>();

        return services;
    }
}
