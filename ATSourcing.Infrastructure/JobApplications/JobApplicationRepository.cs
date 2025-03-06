using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Domain.JobApplications;
using ATSourcing.Domain.JobApplications.Snapshots;
using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure;

namespace ATSourcing.Infrastructure.JobApplications;

internal class JobApplicationRepository : BaseRepository<JobApplication, JobApplicationSnapshot, Guid>, IJobApplicationRepository
{
    public JobApplicationRepository(IDomainEventDispatcher domainEventDispatcher, 
        ISnapshotRepository snapshotRepository, 
        IDomainEventRepository domainEventRepository) : base(domainEventDispatcher, snapshotRepository, domainEventRepository)
    {
    }

    public override Task<JobApplication> CreateAggregateFromSnaphotAndEvents(JobApplicationSnapshot? snapshot, 
        List<IDomainEvent<Guid>> events, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new JobApplication(events, snapshot));
    }

    protected override Task<Guid> CreateKeyAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}
