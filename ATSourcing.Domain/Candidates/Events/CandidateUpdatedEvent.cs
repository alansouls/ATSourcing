using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Candidates.Events;

public class CandidateUpdatedEvent : IDomainEvent<Guid, Dictionary<string, string>>
{
    public CandidateUpdatedEvent(Guid candidateId,
        Dictionary<string, string> updatedData,
        DateTimeOffset timeStamp)
    {
        AggregateId = candidateId;
        Data = updatedData;
        TimeStamp = timeStamp;
    }

    public DateTimeOffset TimeStamp { get; }
    public string Name => nameof(CandidateUpdatedEvent);
    public Guid AggregateId { get; }
    public Dictionary<string, string> Data { get; }
    object IDomainEvent<Guid>.Data => Data;
}