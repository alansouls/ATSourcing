namespace ESFrame.Domain.Interfaces;

public interface IDomainEvent
{
    DateTimeOffset TimeStamp { get; }

    string Name { get; }
}

public interface IDomainEvent<TKey> : IDomainEvent where TKey : IEquatable<TKey>
{
    TKey AggregateId { get; }
}

public interface IDomainEventWithData<TKey> : IDomainEvent<TKey> where TKey : IEquatable<TKey>
{
    object GenericData { get; }
}

public interface IDomainEvent<TKey, out TData> : IDomainEventWithData<TKey> where TData : class where TKey : IEquatable<TKey>
{ 
    TData Data { get; }
}

public abstract class DomainEvent<TKey, TData> : IDomainEvent<TKey, TData> where TData : class where TKey : IEquatable<TKey>
{
    public TData Data { get; protected set; } = null!;

    public TKey AggregateId { get; protected set; } = default!;

    public DateTimeOffset TimeStamp { get; protected set; }

    public abstract string Name { get; }

    object IDomainEventWithData<TKey>.GenericData => Data;
}
