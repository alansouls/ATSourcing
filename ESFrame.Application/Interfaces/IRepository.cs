using ESFrame.Domain.Interfaces;
using FluentResults;

namespace ESFrame.Application.Interfaces;

public interface IRepository<TAggregate, TSnapshot, TKey> where TAggregate : IAggregateRoot<TSnapshot, TKey> where TKey : IEquatable<TKey>
    where TSnapshot : IEntitySnapshot<TKey>
{
    Task<TAggregate?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task<Result> SaveChangesAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
}
