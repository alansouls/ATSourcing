using ATSourcing.Domain.StepDefinitions.Snapshots;
using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.JobApplications.Snapshots;

public class JobApplicationSnapshot : IEntitySnapshot<Guid>
{
    public Guid Id { get; set; }

    public Guid AggregateId { get; set; }

    public DateTimeOffset TimeStamp { get; set; }

    public Guid CandidateId { get; set; }

    public Guid JobId { get; set; }

    public List<StepSnapshot> StepHistory { get; set; } = null!;
}
