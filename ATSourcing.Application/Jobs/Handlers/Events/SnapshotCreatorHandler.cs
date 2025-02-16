using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Domain.Jobs;
using ATSourcing.Domain.Jobs.Events;
using ATSourcing.Domain.Jobs.Snapshots;
using ESFrame.Application;
using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using MediatR;

namespace ATSourcing.Application.Jobs.Handlers.Events;

internal class SnapshotCreatorHandler : INotificationHandler<ApplicationNotification<JobCreatedEvent>>,
    INotificationHandler<ApplicationNotification<JobUpdatedEvent>>,
    INotificationHandler<ApplicationNotification<JobDeletedEvent>>
{
    private const int SnapshotThreshold = 5;
    private readonly IJobRepository _jobRepository;
    private readonly IDomainEventRepository _domainEventRepository;
    private readonly ISnapshotRepository _snapshotRepository;

    public SnapshotCreatorHandler(IJobRepository jobRepository, 
        IDomainEventRepository domainEventRepository, 
        ISnapshotRepository snapshotRepository)
    {
        _jobRepository = jobRepository;
        _domainEventRepository = domainEventRepository;
        _snapshotRepository = snapshotRepository;
    }

    public Task Handle(ApplicationNotification<JobCreatedEvent> notification, CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    public Task Handle(ApplicationNotification<JobUpdatedEvent> notification, CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    public Task Handle(ApplicationNotification<JobDeletedEvent> notification, CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    public async Task GenerateSnapshot(IDomainEvent<Guid> domainEvent, CancellationToken cancellationToken)
    {
        var lastEventTimeStamp = await _domainEventRepository.GetLastDomainEventTimeStampByAggregateId<Job, Guid>
            (domainEvent.AggregateId, cancellationToken);

        if (lastEventTimeStamp is not null && 
            lastEventTimeStamp.Value > domainEvent.TimeStamp)
        {
            return;
        }

        var eventCount = await _domainEventRepository.CountDomainEventsByAggregateId<Job, Guid>
            (domainEvent.AggregateId, domainEvent.TimeStamp, cancellationToken);

        if (eventCount < SnapshotThreshold)
        {
            return;
        }

        var job = await _jobRepository.GetByIdAsync(domainEvent.AggregateId, cancellationToken);

        if (job is null)
        {
            return;
        }

        var snapshot = job.CreateSnapshot();

        snapshot.TimeStamp = domainEvent.TimeStamp;

        await _snapshotRepository.SaveAsync<Job, JobSnapshot, Guid>(snapshot, cancellationToken);
    }
}
