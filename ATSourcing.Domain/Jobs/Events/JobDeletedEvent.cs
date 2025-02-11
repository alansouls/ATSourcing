using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Jobs.Events;

public class JobDeletedEvent : IDomainEvent<Guid>
{
    public JobDeletedEvent(Guid candidateId, DateTimeOffset timeStamp)
    {
        AggregateId = candidateId;
        TimeStamp = timeStamp;
    }

    public DateTimeOffset TimeStamp { get; }
    public string Name => nameof(JobDeletedEvent);
    public Guid AggregateId { get; }
}