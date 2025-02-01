using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Domain.Candidates;
using ATSourcing.Domain.Candidates.Snapshots;
using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure;
using ESFrame.Insfrastructure.Interfaces;

namespace ATSourcing.Infrastructure.Candidates;

public class CandidateRepository : BaseRepository<Candidate, Guid, CandidateSnapshot>, ICandidateRepository
{
    public CandidateRepository(IDomainEventDispatcher domainEventDispatcher,
        ISnapshotRepository snapshotRepository, 
        IDomainEventRepository domainEventRepository) : base(domainEventDispatcher, snapshotRepository, domainEventRepository)
    {
    }

    public override Task<Candidate> CreateAggregateFromSnaphotAndEvents(CandidateSnapshot? snapshot, List<IDomainEvent<Guid>> events,
        CancellationToken cancellationToken = default)
    {
        var candidate = new Candidate(events, snapshot)
    }

    protected override Task<Guid> CreateKeyAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}