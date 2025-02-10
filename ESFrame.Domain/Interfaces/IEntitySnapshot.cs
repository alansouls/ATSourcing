namespace ESFrame.Domain.Interfaces;

public interface IEntitySnapshot<TKey> where TKey : IEquatable<TKey>
{
    public Guid Id { get; }
    TKey AggregateId { get; }

    DateTimeOffset TimeStamp { get; }
}
