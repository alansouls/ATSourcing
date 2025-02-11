using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Domain.Candidates;
using ATSourcing.Domain.Candidates.Snapshots;
using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure;

namespace ATSourcing.Infrastructure.Candidates;

public class CandidateRepository : BaseRepository<Candidate, CandidateSnapshot, Guid>, ICandidateRepository
{
    public CandidateRepository(IDomainEventDispatcher domainEventDispatcher,
        ISnapshotRepository snapshotRepository, 
        IDomainEventRepository domainEventRepository) : base(domainEventDispatcher, snapshotRepository, domainEventRepository)
    {
    }

    public override Task<Candidate> CreateAggregateFromSnaphotAndEvents(CandidateSnapshot? snapshot, List<IDomainEvent<Guid>> events,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Candidate(events, snapshot));
    }

    protected override Task<Guid> CreateKeyAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}