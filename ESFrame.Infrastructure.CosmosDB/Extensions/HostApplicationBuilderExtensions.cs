﻿using ESFrame.Application.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Internal;
using ESFrame.Infrastructure.CosmosDB.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ESFrame.Infrastructure.CosmosDB.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddCosmosInfrastructure(this IHostApplicationBuilder builder, string connectionName)
    {
        builder.AddAzureCosmosClient(connectionName, 
            configureClientOptions: options =>
            {
                options.LimitToEndpoint = true;
                options.ConnectionMode = ConnectionMode.Gateway;
                options.SerializerOptions = new CosmosSerializationOptions
                {
                    IgnoreNullValues = false,
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                };
            });

        builder.Services.AddScoped<IContainerFactory, ContainerFactory>();

        builder.Services.AddScoped<IDomainEventRepository, DomainEventCosmosRepository>();

        builder.Services.AddScoped<ISnapshotRepository, SnapshotRepository>();

        builder.Services.Configure<CosmosOptions>(builder.Configuration
            .GetSection(CosmosOptions.OptionsName));

        return builder;
    }
}
