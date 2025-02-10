using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Domain.Candidates;
using ATSourcing.Domain.Candidates.Events;
using ATSourcing.Domain.Candidates.Snapshots;
using ESFrame.Application;
using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using MediatR;

namespace ATSourcing.Application.Candidates.Handlers.Events;

internal class SnapshotCreatorHandler : INotificationHandler<ApplicationNotification<CandidateCreatedEvent>>,
    INotificationHandler<ApplicationNotification<CandidateUpdatedEvent>>,
    INotificationHandler<ApplicationNotification<CandidateDeletedEvent>>
{
    private const int SnapshotThreshold = 5;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IDomainEventRepository _domainEventRepository;
    private readonly ISnapshotRepository _snapshotRepository;

    public SnapshotCreatorHandler(ICandidateRepository candidateRepository, 
        IDomainEventRepository domainEventRepository, 
        ISnapshotRepository snapshotRepository)
    {
        _candidateRepository = candidateRepository;
        _domainEventRepository = domainEventRepository;
        _snapshotRepository = snapshotRepository;
    }

    public Task Handle(ApplicationNotification<CandidateCreatedEvent> notification, CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    public Task Handle(ApplicationNotification<CandidateUpdatedEvent> notification, CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    public Task Handle(ApplicationNotification<CandidateDeletedEvent> notification, CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    public async Task GenerateSnapshot(IDomainEvent<Guid> domainEvent, CancellationToken cancellationToken)
    {
        var lastEventTimeStamp = await _domainEventRepository.GetLastDomainEventTimeStampByAggregateId<Candidate, Guid>
            (domainEvent.AggregateId, cancellationToken);

        if (lastEventTimeStamp is not null && 
            lastEventTimeStamp.Value > domainEvent.TimeStamp)
        {
            return;
        }

        var eventCount = await _domainEventRepository.CountDomainEventsByAggregateId<Candidate, Guid>
            (domainEvent.AggregateId, domainEvent.TimeStamp, cancellationToken);

        if (eventCount < SnapshotThreshold)
        {
            return;
        }

        var candidate = await _candidateRepository.GetByIdAsync(domainEvent.AggregateId, cancellationToken);

        if (candidate is null)
        {
            return;
        }

        var snapshot = candidate.CreateSnapshot();

        snapshot.TimeStamp = domainEvent.TimeStamp;

        await _snapshotRepository.SaveAsync<Candidate, CandidateSnapshot, Guid>(snapshot, cancellationToken);
    }
}
