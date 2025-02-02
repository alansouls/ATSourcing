using ATSourcing.Application.Candidates.Interfaces;
using ESFrame.Insfrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ATSourcing.Infrastructure.Candidates.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCandidateInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddSingleton<IDomainEventConverterModule, DomainEventConverterModule>();

        return services;
    }
}
