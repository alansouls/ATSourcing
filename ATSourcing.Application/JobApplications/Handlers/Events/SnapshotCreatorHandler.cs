using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Domain.JobApplications;
using ATSourcing.Domain.JobApplications.Events;
using ATSourcing.Domain.JobApplications.Snapshots;
using ESFrame.Application;
using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using MediatR;

namespace ATSourcing.Application.JobApplications.Handlers.Events;

internal class SnapshotCreatorHandler : INotificationHandler<ApplicationNotification<JobApplicationCreatedEvent>>,
    INotificationHandler<ApplicationNotification<JobApplicationAnswerAddedEvent>>,
    INotificationHandler<ApplicationNotification<JobApplicationCurrentStepApprovedEvent>>,
    INotificationHandler<ApplicationNotification<JobApplicationCurrentStepRejectedEvent>>
{
    private const int SnapshotThreshold = 5;
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IDomainEventRepository _domainEventRepository;
    private readonly ISnapshotRepository _snapshotRepository;

    public SnapshotCreatorHandler(IJobApplicationRepository jobApplicationRepository,
        IDomainEventRepository domainEventRepository,
        ISnapshotRepository snapshotRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _domainEventRepository = domainEventRepository;
        _snapshotRepository = snapshotRepository;
    }

    public Task Handle(ApplicationNotification<JobApplicationCreatedEvent> notification,
        CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    public Task Handle(ApplicationNotification<JobApplicationAnswerAddedEvent> notification,
        CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    public Task Handle(ApplicationNotification<JobApplicationCurrentStepApprovedEvent> notification,
        CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    public Task Handle(ApplicationNotification<JobApplicationCurrentStepRejectedEvent> notification,
        CancellationToken cancellationToken) =>
        GenerateSnapshot(notification.Event, cancellationToken);

    private async Task GenerateSnapshot(IDomainEvent<Guid> domainEvent, CancellationToken cancellationToken)
    {
        var lastEventTimeStamp =
            await _domainEventRepository.GetLastDomainEventTimeStampByAggregateId<JobApplication, Guid>
                (domainEvent.AggregateId, cancellationToken);

        if (lastEventTimeStamp is not null &&
            lastEventTimeStamp.Value > domainEvent.TimeStamp)
        {
            return;
        }

        var eventCount = await _domainEventRepository.CountDomainEventsByAggregateId<JobApplication, Guid>
            (domainEvent.AggregateId, domainEvent.TimeStamp, cancellationToken);

        if (eventCount < SnapshotThreshold)
        {
            return;
        }

        var job = await _jobApplicationRepository.GetByIdAsync(domainEvent.AggregateId, cancellationToken);

        if (job is null)
        {
            return;
        }

        var snapshot = job.CreateSnapshot();

        snapshot.TimeStamp = domainEvent.TimeStamp;

        await _snapshotRepository.SaveAsync<JobApplication, JobApplicationSnapshot, Guid>(snapshot, cancellationToken);
    }
}