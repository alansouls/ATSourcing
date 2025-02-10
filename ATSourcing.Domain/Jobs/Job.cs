using ATSourcing.Domain.Jobs.Snapshots;
using ESFrame.Domain;
using ESFrame.Domain.Interfaces;
using FluentResults;

namespace ATSourcing.Domain.Jobs;

public class Job : BaseAggregateRoot<JobSnapshot, Guid>
{
    public string Title { get; private set; }

    public string Description { get; private set; }

    private readonly List<Guid> _candidates = new();
    public IReadOnlyList<Guid> Candidates => _candidates;

    public override JobSnapshot CreateSnapshot()
    {
        throw new NotImplementedException();
    }

    protected override Result ApplyEventEffect(IDomainEvent<Guid> domainEvent)
    {
        throw new NotImplementedException();
    }

    protected override void RestoreSnapshot(JobSnapshot snapshot)
    {
        throw new NotImplementedException();
    }
}
