using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Candidates.Events;

public record CandidateCreatedEventData(string FirstName, string LastName, int Age, string Email, Guid UserId);

public class CandidateCreatedEvent : IDomainEvent<Guid, CandidateCreatedEventData>
{
    private readonly Candidate _candidate;
    
    public CandidateCreatedEvent(Candidate candidate, CandidateCreatedEventData data, DateTimeOffset createdDate)
    {
        _candidate = candidate;
        Data = data;
        TimeStamp = createdDate;
    }
    public DateTimeOffset TimeStamp { get; }
    public string Name => nameof(CandidateCreatedEvent);
    public Guid AggregateId => _candidate.Id;
    
    object IDomainEvent<Guid>.Data => Data;
    public CandidateCreatedEventData Data { get; }
}