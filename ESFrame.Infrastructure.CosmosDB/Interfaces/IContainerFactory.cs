using Microsoft.Azure.Cosmos;

namespace ESFrame.Infrastructure.CosmosDB.Interfaces;

public interface IContainerFactory
{
    Task<Container> GetOrCreateDomainContainerAsync(string containerName, CancellationToken cancellationToken);
    Task<Container> GetOrCreateViewContainerAsync(string containerName, string partitionKeyPath, CancellationToken cancellationToken);
}
