namespace ESFrame.Domain.Interfaces;

public interface IEntitySnapshot<TKey> where TKey : IEquatable<TKey>
{
    TKey AggregateId { get; }

    DateTimeOffset TimeStamp { get; }
}
