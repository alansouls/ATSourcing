using ATSourcing.Domain.StepDefinitions.Definitions.Snapshots;
using ATSourcing.Domain.ValueObjects;
using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Jobs.Snapshots;

public class JobSnapshot : IEntitySnapshot<Guid>
{
    public Guid Id { get; set; }

    public Guid AggregateId { get; set; }

    public DateTimeOffset TimeStamp { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required List<Guid> Candidates { get; set; }

    public DateTimeOffset ApplicationDeadline { get; set; }

    public int VacancyCount { get; set; }

    public DecimalRange? SalaryRange { get; set; }

    public required StepFlowDefinitionSnapshot StepFlow { get; set; }
}