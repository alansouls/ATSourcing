using ESFrame.Domain.Interfaces;

namespace ESFrame.Insfrastructure.Interfaces;

public interface IDomainEventRepository
{
    Task<List<IDomainEvent<TKey>>> GetDomainEventsByAggregateId<TAggregate, TKey>( 
        TKey aggregateId, 
        DateTimeOffset? from,
        CancellationToken cancellationToken)
        where TAggregate : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>;

    Task SaveAsync<TAggregate, TKey>(
        TKey aggregateId, 
        IEnumerable<IDomainEvent<TKey>> events,
        CancellationToken cancellationToken)
        where TAggregate : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>;
}
