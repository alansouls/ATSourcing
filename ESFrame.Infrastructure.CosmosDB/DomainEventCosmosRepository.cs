using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using ESFrame.Infrastructure.CosmosDB.Interfaces;
using ESFrame.Insfrastructure.Extensions;
using ESFrame.Insfrastructure.Interfaces;
using ESFrame.Insfrastructure.Models;
using Microsoft.Azure.Cosmos;

namespace ESFrame.Infrastructure.CosmosDB;

internal class DomainEventCosmosRepository : IDomainEventRepository
{
    private const string EventsSuffix = "_Events";
    private readonly IContainerFactory _containerFactory;
    private readonly IDomainEventModelConverter _domainEventModelConverter;

    public DomainEventCosmosRepository(IContainerFactory containerFactory, IDomainEventModelConverter domainEventModelConverter)
    {
        _containerFactory = containerFactory;
        _domainEventModelConverter = domainEventModelConverter;
    }

    public async Task<int> CountDomainEventsByAggregateId<TAggregate, TKey>(TKey aggregateId, DateTimeOffset? from, CancellationToken cancellationToken)
        where TAggregate : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        var container = await _containerFactory.GetOrCreateDomainContainerAsync(typeof(TAggregate).Name + EventsSuffix,
            cancellationToken);

        var iter = container.GetItemQueryIterator<int>(new QueryDefinition(
            $"SELECT VALUE COUNT(c.id) FROM c WHERE c.aggregateId = '{aggregateId}' AND c.timeStamp > '{from}'"));

        return (await iter.ReadNextAsync(cancellationToken))
            .First();
    }

    public async Task<List<IDomainEvent<TKey>>> GetDomainEventsByAggregateId<TAggregate, TKey>(
        TKey aggregateId, DateTimeOffset? from, CancellationToken cancellationToken) 
        where TKey : IEquatable<TKey>
        where TAggregate : IAggregateRoot<TKey>
    {
        var container = await _containerFactory.GetOrCreateDomainContainerAsync(typeof(TAggregate).Name + EventsSuffix, 
            cancellationToken);

        var query = from.HasValue
            ? $"SELECT * FROM c WHERE c.aggregateId = '{aggregateId}' AND c.timeStamp > '{from.Value:o}'"
            : $"SELECT * FROM c WHERE c.aggregateId = '{aggregateId}'";

        var iter = container.GetItemQueryIterator<DomainEventModel>(new QueryDefinition(query),
            requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(aggregateId.ToString())
            });

        var result = new List<IDomainEvent<TKey>>();
        while (iter.HasMoreResults)
        {
            var response = await iter.ReadNextAsync(cancellationToken);
            result.AddRange(response.Select(_domainEventModelConverter.ToDomainEvent<TKey>));
        }

        return result;
    }

    public async Task<DateTimeOffset?> GetLastDomainEventTimeStampByAggregateId<TAggregate, TKey>(TKey aggregateId, CancellationToken cancellationToken)
        where TAggregate : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        var container = await _containerFactory.GetOrCreateDomainContainerAsync(typeof(TAggregate).Name + EventsSuffix,
            cancellationToken);

        var iter = container.GetItemQueryIterator<DomainEventModel>(new QueryDefinition(
            $"SELECT TOP 1 * FROM c WHERE c.aggregateId = '{aggregateId}' ORDER BY c.timeStamp DESC"));

        return (await iter.ReadNextAsync(cancellationToken))
            .FirstOrDefault()?.TimeStamp;
    }

    public async Task SaveAsync<TAggregate, TKey>(TKey aggregateId, 
        IEnumerable<IDomainEvent<TKey>> events, CancellationToken cancellationToken) 
        where TKey : IEquatable<TKey>
        where TAggregate : IAggregateRoot<TKey>
    {
        if (!events.Any())
        {
            return;
        }

        var container = await _containerFactory.GetOrCreateDomainContainerAsync(typeof(TAggregate).Name + EventsSuffix,
            cancellationToken);

        var batch = container.CreateTransactionalBatch(new PartitionKey(aggregateId.ToString()));

        foreach (var @event in events)
        {
            batch.CreateItem(@event.ToModel());
        }

        var response = await batch.ExecuteAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Could not save events");
        }
    }
}
