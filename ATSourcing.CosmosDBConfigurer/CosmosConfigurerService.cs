﻿using ESFrame.Infrastructure.CosmosDB.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSourcing.CosmosDBConfigurer;

internal class CosmosConfigurerService : IHostedService
{
    private readonly IOptions<CosmosOptions> _cosmosOptions;
    private readonly CosmosClient _cosmosClient;

    public CosmosConfigurerService(CosmosClient cosmosClient, IOptions<CosmosOptions> cosmosOptions)
    {
        _cosmosClient = cosmosClient;
        _cosmosOptions = cosmosOptions;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _cosmosClient.CreateDatabaseIfNotExistsAsync(_cosmosOptions.Value.DatabaseId, cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
