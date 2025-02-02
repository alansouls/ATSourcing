using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Candidates.Events;

public record CandidateUpdatedEventData(string FieldName, string NewValue);

public class CandidateUpdatedEvent : DomainEvent<Guid, CandidateUpdatedEventData>
{
    public CandidateUpdatedEvent(Guid candidateId,
        CandidateUpdatedEventData data,
        DateTimeOffset timeStamp)
    {
        AggregateId = candidateId;
        Data = data;
        TimeStamp = timeStamp;
    }
    
    public override string Name => nameof(CandidateUpdatedEvent);
}