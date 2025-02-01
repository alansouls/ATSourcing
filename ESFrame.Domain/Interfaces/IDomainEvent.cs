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

public interface IDomainEvent<TKey, out TData> : IDomainEvent<TKey> where TData : class where TKey : IEquatable<TKey>
{ 
    new TData Data { get; }
}
