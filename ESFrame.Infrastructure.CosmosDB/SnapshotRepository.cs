using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Interfaces;
using Microsoft.Azure.Cosmos;

namespace ESFrame.Infrastructure.CosmosDB;

internal class SnapshotRepository : ISnapshotRepository
{
    private const string SnapshotsSuffix = "_Snapshots";
    private readonly IContainerFactory _containerFactory;

    public SnapshotRepository(IContainerFactory containerFactory)
    {
        _containerFactory = containerFactory;
    }

    public async Task<TSnapshot?> GetAsync<TAggregate, TSnapshot, TKey>(TKey aggregateId, 
        CancellationToken cancellationToken)
        where TSnapshot : class, IEntitySnapshot<TKey> where TKey : IEquatable<TKey>
        where TAggregate : IAggregateRoot<TKey>
    {
        var container = await _containerFactory.GetOrCreateContainerAsync(typeof(TAggregate).Name + SnapshotsSuffix, 
            cancellationToken);

        var iter = container.GetItemQueryIterator<TSnapshot>(new QueryDefinition(
            $"SELECT TOP 1 * FROM c WHERE c.aggregateId = '{aggregateId}' order by c.TimeStamp desc"));

        return (await iter.ReadNextAsync(cancellationToken))
            .FirstOrDefault();
    }

    public async Task SaveAsync<TAggregate, TSnapshot, TKey>(TSnapshot aggregate, 
        CancellationToken cancellationToken)
        where TSnapshot : class, IEntitySnapshot<TKey>
        where TKey : IEquatable<TKey>
        where TAggregate : IAggregateRoot<TKey>
    {
        var container = await _containerFactory.GetOrCreateContainerAsync(typeof(TAggregate).Name + SnapshotsSuffix,
            cancellationToken);

        await container.CreateItemAsync(aggregate, 
            new PartitionKey(aggregate.AggregateId.ToString()), 
            cancellationToken: cancellationToken);
    }
}
