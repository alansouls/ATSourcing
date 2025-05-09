﻿using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using FluentResults;

namespace ESFrame.Insfrastructure;

public abstract class BaseRepository<TAggregate, TSnapshot, TKey> : IRepository<TAggregate, TSnapshot, TKey> 
    where TAggregate : class, IAggregateRoot<TSnapshot, TKey> 
    where TKey : IEquatable<TKey> 
    where TSnapshot : class, IEntitySnapshot<TKey>
{
    private readonly IDomainEventRepository _domainEventRepository;
    private readonly ISnapshotRepository _snapshotRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    protected BaseRepository(IDomainEventDispatcher domainEventDispatcher,
        ISnapshotRepository snapshotRepository,
        IDomainEventRepository domainEventRepository)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _snapshotRepository = snapshotRepository;
        _domainEventRepository = domainEventRepository;
    }

    public async Task<TAggregate?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var snapshot = await _snapshotRepository.GetAsync<TAggregate, TSnapshot, TKey>(id, cancellationToken);

        var domainEvents = await _domainEventRepository.GetDomainEventsByAggregateId<TAggregate, TKey>(
            id, 
            snapshot?.TimeStamp,
            cancellationToken);

        if (snapshot is null && domainEvents.Count == 0)
        {
            return null;
        }

        return await CreateAggregateFromSnaphotAndEvents(snapshot, domainEvents, cancellationToken);
    }

    public abstract Task<TAggregate> CreateAggregateFromSnaphotAndEvents(TSnapshot? snapshot, 
        List<IDomainEvent<TKey>> events, 
        CancellationToken cancellationToken = default);

    public async Task<Result> SaveChangesAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var result = await BeforeSaveChangesAsync(aggregate, cancellationToken);

        if (result.IsFailed)
            return result;

        await AttributeAggregateKeyAsync(aggregate, cancellationToken);

        var domainEvents = aggregate.DomainEvents
            .ToList();

        aggregate.ClearDomainEvents();

        if (domainEvents.Count == 0)
            return Result.Ok();

        await _domainEventRepository.SaveAsync<TAggregate, TKey>(aggregate.Id!, domainEvents, cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _domainEventDispatcher.DispatchAsync(domainEvent, CancellationToken.None);
        }

        return Result.Ok();
    }

    protected virtual async Task AttributeAggregateKeyAsync(TAggregate aggregate, CancellationToken cancellationToken)
    {
        if (aggregate.Id is null || aggregate.Id.Equals(default(TKey)))
        {
            var key = await CreateKeyAsync(cancellationToken);
            aggregate.GetType().GetProperty(nameof(aggregate.Id))?.SetValue(aggregate, key);
        }
    }

    protected abstract Task<TKey> CreateKeyAsync(CancellationToken cancellationToken);

    protected virtual Task<Result> BeforeSaveChangesAsync(TAggregate aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok());
    }
}
