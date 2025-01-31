using Microsoft.Azure.Cosmos;

namespace ESFrame.Infrastructure.CosmosDB.Interfaces;

public interface IContainerFactory
{
    Task<Container> GetOrCreateContainerAsync(string containerName, CancellationToken cancellationToken);
}
