using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Candidates.Events;

public record CandidateCreatedEventData(string FirstName, string LastName, int Age, string Email, Guid UserId);

public class CandidateCreatedEvent : IDomainEvent<Guid, CandidateCreatedEventData>
{
    public CandidateCreatedEvent(Guid candidateId, CandidateCreatedEventData data, DateTimeOffset createdDate)
    {
        AggregateId = candidateId;
        Data = data;
        TimeStamp = createdDate;
    }
    
    public DateTimeOffset TimeStamp { get; }
    public string Name => nameof(CandidateCreatedEvent);
    public Guid AggregateId { get; }
    public CandidateCreatedEventData Data { get; }
}