using ESFrame.Domain.Interfaces;

namespace ESFrame.Application.Interfaces;

public interface ISnapshotRepository
{
    Task<TSnapshot?> GetAsync<TAggregate, TSnapshot, TKey>(TKey aggregateId, CancellationToken cancellationToken)
        where TSnapshot : class, IEntitySnapshot<TKey> where TKey : IEquatable<TKey>
        where TAggregate : IAggregateRoot<TKey>;

    Task SaveAsync<TAggregate, TSnapshot, TKey>(TSnapshot aggregate, CancellationToken cancellationToken)
        where TSnapshot : class, IEntitySnapshot<TKey>
        where TKey : IEquatable<TKey>
        where TAggregate : IAggregateRoot<TKey>;
}
