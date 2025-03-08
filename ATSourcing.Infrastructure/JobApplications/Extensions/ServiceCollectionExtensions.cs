using ATSourcing.Application.JobApplications.Interfaces;
using ESFrame.Insfrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ATSourcing.Infrastructure.JobApplications.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobApplicationInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
        services.AddScoped<IJobApplicationViewService, JobApplicationViewService>();
        services.AddSingleton<IDomainEventConverterModule, DomainEventConverterModule>();

        return services;
    }
}
