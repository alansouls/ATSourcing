using ESFrame.Domain.Interfaces;

namespace ESFrame.Application.Interfaces;

public interface IDomainEventRepository
{
    Task<DateTimeOffset?> GetLastDomainEventTimeStampByAggregateId<TAggregate, TKey>(
        TKey aggregateId,
        CancellationToken cancellationToken )
        where TAggregate : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>;

    Task<int> CountDomainEventsByAggregateId<TAggregate, TKey>(
        TKey aggregateId,
        DateTimeOffset? from,
        CancellationToken cancellationToken)
        where TAggregate : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>;

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
