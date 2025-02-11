using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Jobs.Events;

public record JobUpdatedEventData(string FieldName, string NewValue);

public class JobUpdatedEvent : DomainEvent<Guid, JobUpdatedEventData>
{
    public JobUpdatedEvent(Guid candidateId,
        JobUpdatedEventData data,
        DateTimeOffset timeStamp)
    {
        AggregateId = candidateId;
        Data = data;
        TimeStamp = timeStamp;
    }

    public override string Name => nameof(JobUpdatedEvent);
}