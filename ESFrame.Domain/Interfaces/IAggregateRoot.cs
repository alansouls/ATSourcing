namespace ESFrame.Domain.Interfaces;

public interface IAggregateRoot<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
{
    IReadOnlyList<IDomainEvent<TKey>> DomainEvents { get; }
    void ClearDomainEvents();
}

public interface IAggregateRoot<TSnapshot, TKey> : IAggregateRoot<TKey> where TKey : IEquatable<TKey> where TSnapshot : IEntitySnapshot<TKey>
{
    TSnapshot CreateSnapshot();
}
