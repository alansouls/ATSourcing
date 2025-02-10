using ESFrame.Infrastructure.CosmosDB.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace ESFrame.Infrastructure.CosmosDB.Internal;

internal class ContainerFactory : IContainerFactory
{
    private SemaphoreSlim _domainContainerSemaphore = new(1, 1);
    private SemaphoreSlim _viewContainerSemaphore = new(1, 1);
    private readonly CosmosClient _cosmosClient;
    private readonly IOptions<CosmosOptions> _options;
    private readonly Dictionary<string, Container> _domainContainersByAggregateName;
    private readonly Dictionary<string, Container> _viewContainersByName;

    public ContainerFactory(CosmosClient cosmosClient, IOptions<CosmosOptions> options)
    {
        _cosmosClient = cosmosClient;
        _options = options;
        _domainContainersByAggregateName = [];
        _viewContainersByName = [];
    }

    public async Task<Container> GetOrCreateDomainContainerAsync(string containerName, CancellationToken cancellationToken)
    {
        await _domainContainerSemaphore.WaitAsync(cancellationToken);

        try
        {
            var container = _domainContainersByAggregateName.GetValueOrDefault(containerName);

            if (container is null)
            {
                var database = _cosmosClient.GetDatabase(_options.Value.DomainDatabaseId);

                var response = await database.CreateContainerIfNotExistsAsync(new ContainerProperties
                {
                    Id = containerName,
                    PartitionKeyPath = "/aggregateId",
                }, cancellationToken: cancellationToken);

                if (response.Container is null)
                {
                    throw new Exception("Container not created");
                }

                container = response.Container;

                _domainContainersByAggregateName.Add(containerName, container);
            }

            return container;
        }
        finally
        {
            _domainContainerSemaphore.Release();
        }
    }

    public async Task<Container> GetOrCreateViewContainerAsync(string containerName, string partitionKeyPath, CancellationToken cancellationToken)
    {
        await _viewContainerSemaphore.WaitAsync(cancellationToken);

        try
        {
            var container = _viewContainersByName.GetValueOrDefault(containerName);

            if (container is null)
            {
                var database = _cosmosClient.GetDatabase(_options.Value.ViewDatabaseId);

                var response = await database.CreateContainerIfNotExistsAsync(new ContainerProperties
                {
                    Id = containerName,
                    PartitionKeyPath = partitionKeyPath
                }, cancellationToken: cancellationToken);

                if (response.Container is null)
                {
                    throw new Exception("Container not created");
                }

                container = response.Container;

                _viewContainersByName.Add(containerName, container);
            }

            return container;
        }
        finally
        {
            _viewContainerSemaphore.Release();
        }
    }
}
