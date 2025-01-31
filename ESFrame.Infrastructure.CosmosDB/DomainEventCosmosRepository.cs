using ESFrame.Domain.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Options;
using ESFrame.Insfrastructure.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace ESFrame.Infrastructure.CosmosDB;

internal class DomainEventCosmosRepository : IDomainEventRepository
{
    private const string EventsSuffix = "_Events";
    private readonly IContainerFactory _containerFactory;

    public DomainEventCosmosRepository(IContainerFactory containerFactory)
    {
        _containerFactory = containerFactory;
    }

    public async Task<List<IDomainEvent<TKey>>> GetDomainEventsByAggregateId<TAggregate, TKey>(
        TKey aggregateId, DateTimeOffset? from, CancellationToken cancellationToken) 
        where TKey : IEquatable<TKey>
        where TAggregate : IAggregateRoot<TKey>
    {
        var container = await _containerFactory.GetOrCreateContainerAsync(typeof(TAggregate).Name + EventsSuffix, 
            cancellationToken);

        var iter = container.GetItemQueryIterator<IDomainEvent<TKey>>(new QueryDefinition(
            $"SELECT * FROM c WHERE c.aggregateId = '{aggregateId}' AND c.timeStamp > '{from}'"));

        var result = new List<IDomainEvent<TKey>>();
        while (iter.HasMoreResults)
        {
            var response = await iter.ReadNextAsync(cancellationToken);
            result.AddRange(response);
        }

        return result;
    }

    public async Task SaveAsync<TAggregate, TKey>(TKey aggregateId, 
        IEnumerable<IDomainEvent<TKey>> events, CancellationToken cancellationToken) 
        where TKey : IEquatable<TKey>
        where TAggregate : IAggregateRoot<TKey>
    {
        var container = await _containerFactory.GetOrCreateContainerAsync(typeof(TAggregate).Name + EventsSuffix,
            cancellationToken);

        var batch = container.CreateTransactionalBatch(new PartitionKey(aggregateId.ToString()));

        foreach (var @event in events)
        {
            batch.CreateItem(@event);
        }

        var response = await batch.ExecuteAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Could not save events");
        }
    }
}
