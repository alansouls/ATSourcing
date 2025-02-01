namespace ESFrame.Domain.Interfaces;

public interface IEntity<TKey> where TKey : IEquatable<TKey>
{
    TKey? Id { get; }
    
    bool IsDeleted { get; }
}