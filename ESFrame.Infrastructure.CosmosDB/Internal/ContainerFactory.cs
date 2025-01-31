using ESFrame.Infrastructure.CosmosDB.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace ESFrame.Infrastructure.CosmosDB.Internal;

internal class ContainerFactory : IContainerFactory
{
    private SemaphoreSlim _containerSemaphore = new(1, 1);
    private readonly CosmosClient _cosmosClient;
    private readonly IOptions<CosmosOptions> _options;
    private readonly Dictionary<string, Container> _containersByAggregateName;

    public ContainerFactory(CosmosClient cosmosClient, IOptions<CosmosOptions> options)
    {
        _cosmosClient = cosmosClient;
        _options = options;
        _containersByAggregateName = [];
    }

    public async Task<Container> GetOrCreateContainerAsync(string containerName, CancellationToken cancellationToken)
    {
        await _containerSemaphore.WaitAsync(cancellationToken);

        try
        {
            var container = _containersByAggregateName.GetValueOrDefault(containerName);

            if (container is null)
            {
                var database = _cosmosClient.GetDatabase(_options.Value.DatabaseId);

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

                _containersByAggregateName.Add(containerName, container);
            }

            return container;
        }
        finally
        {
            _containerSemaphore.Release();
        }
    }
}
