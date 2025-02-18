using ATSourcing.Domain.StepDefinitions.Definitions;
using ATSourcing.Domain.ValueObjects;
using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Jobs.Events;

public record JobCreatedEventData(string Title, 
    string Description, 
    DateTimeOffset ApplicationDeadline, 
    int VacancyCount, 
    DecimalRange? SalaryRange, 
    StepFlowDefinition StepFlow);

public class JobCreatedEvent : DomainEvent<Guid, JobCreatedEventData>
{
    public JobCreatedEvent(Guid candidateId, JobCreatedEventData data, DateTimeOffset createdDate)
    {
        AggregateId = candidateId;
        Data = data;
        TimeStamp = createdDate;
    }

    public override string Name => nameof(JobCreatedEvent);
}