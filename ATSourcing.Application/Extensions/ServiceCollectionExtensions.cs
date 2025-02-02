using ATSourcing.Application.Candidates.Requests.Commands;
using ATSourcing.Domain.Candidates.Events;
using Microsoft.Extensions.DependencyInjection;

namespace ATSourcing.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<CreateCandidateCommand>();
        });

        return services;
    }
}