using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Candidates.Events;

public class CandidateDeletedEvent : IDomainEvent<Guid>
{
    public CandidateDeletedEvent(Guid candidateId, DateTimeOffset timeStamp)
    {
        AggregateId = candidateId;
        TimeStamp = timeStamp;
    }
    
    public DateTimeOffset TimeStamp { get; }
    public string Name => nameof(CandidateDeletedEvent);
    public Guid AggregateId { get; }
}