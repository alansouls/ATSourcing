using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Candidates.Events;

public record CandidateUpdatedEventData(string FieldName, string NewValue);

public class CandidateUpdatedEvent : IDomainEvent<Guid, CandidateUpdatedEventData>
{
    public CandidateUpdatedEvent(Guid candidateId,
        CandidateUpdatedEventData data,
        DateTimeOffset timeStamp)
    {
        AggregateId = candidateId;
        Data = data;
        TimeStamp = timeStamp;
    }

    public DateTimeOffset TimeStamp { get; }
    public string Name => nameof(CandidateUpdatedEvent);
    public Guid AggregateId { get; }
    public CandidateUpdatedEventData Data { get; }
}