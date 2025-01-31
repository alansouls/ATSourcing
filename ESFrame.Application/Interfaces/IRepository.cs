using ESFrame.Domain.Interfaces;
using FluentResults;

namespace ESFrame.Application.Interfaces;

public interface IRepository<TAggregate, TKey> where TAggregate : IAggregateRoot<TKey> where TKey : IEquatable<TKey>
{
    Task<TAggregate?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task<Result> SaveChangesAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
}
