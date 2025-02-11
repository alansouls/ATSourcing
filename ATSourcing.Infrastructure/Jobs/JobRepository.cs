using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Domain.Jobs;
using ATSourcing.Domain.Jobs.Snapshots;
using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure;

namespace ATSourcing.Infrastructure.Jobs;

internal class JobRepository : BaseRepository<Job, JobSnapshot, Guid>, IJobRepository
{
    public JobRepository(IDomainEventDispatcher domainEventDispatcher, 
        ISnapshotRepository snapshotRepository, 
        IDomainEventRepository domainEventRepository) : base(domainEventDispatcher, snapshotRepository, domainEventRepository)
    {
    }

    public override Task<Job> CreateAggregateFromSnaphotAndEvents(JobSnapshot? snapshot, List<IDomainEvent<Guid>> events, 
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Job(events, snapshot));
    }

    protected override Task<Guid> CreateKeyAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}
