using ESFrame.Domain.Interfaces;
using FluentResults;

namespace ESFrame.Domain;

public abstract class BaseAggregateRoot<TSnapshot, TKey> : IAggregateRoot<TSnapshot, TKey> 
    where TKey : IEquatable<TKey> where TSnapshot : IEntitySnapshot<TKey>
{
    private readonly List<IDomainEvent<TKey>> _domainEvents = [];

    public TKey? Id { get; protected set; }

    public bool IsDeleted { get; protected set; } = false;

    public IReadOnlyList<IDomainEvent<TKey>> DomainEvents => _domainEvents;

    protected BaseAggregateRoot()
    {
    }

    protected BaseAggregateRoot(IEnumerable<IDomainEvent<TKey>> domainEvents, TSnapshot? snapshot)
    {
        if (snapshot is not null)
        {
            RestoreSnapshot(snapshot);

            Id = snapshot.AggregateId;
        }

        foreach (var domainEvent in domainEvents.OrderBy(d => d.TimeStamp))
        {
            var result = ApplyEventEffect(domainEvent);

            if (result.IsFailed)
            {
                throw new InvalidOperationException($"Failed to apply event {domainEvent.Name} to entity {GetType().Name} with id {domainEvent.AggregateId}");
            }

            Id = domainEvent.AggregateId;
        }
    }
    
    protected Result AddEvent(IDomainEvent<TKey> domainEvent)
    {
        var result = ApplyEventEffect(domainEvent);

        if (result.IsFailed)
        {
            return result;
        }

        _domainEvents.Add(domainEvent);

        return Result.Ok();
    }

    protected abstract Result ApplyEventEffect(IDomainEvent<TKey> domainEvent);

    protected abstract void RestoreSnapshot(TSnapshot snapshot);

    public abstract TSnapshot CreateSnapshot();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
