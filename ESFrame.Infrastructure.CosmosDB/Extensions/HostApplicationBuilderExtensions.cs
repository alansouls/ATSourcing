using ESFrame.Application.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ESFrame.Infrastructure.CosmosDB.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddCosmosInfrastructure(this IHostApplicationBuilder builder, string connectionName)
    {
        builder.AddAzureCosmosClient(connectionName);

        builder.Services.AddScoped<IContainerFactory, ContainerFactory>();

        builder.Services.AddScoped<IDomainEventRepository, DomainEventCosmosRepository>();

        builder.Services.AddScoped<ISnapshotRepository, SnapshotRepository>();

        return builder;
    }
}
