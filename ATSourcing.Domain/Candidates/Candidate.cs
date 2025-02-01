using ATSourcing.Domain.Candidates.Events;
using ATSourcing.Domain.Candidates.Snapshots;
using ESFrame.Domain;
using ESFrame.Domain.Interfaces;
using FluentResults;

namespace ATSourcing.Domain.Candidates;

public class Candidate : BaseAggregateRoot<CandidateSnapshot, Guid>, IAggregateRoot<Guid>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public int Age { get; private set; }
    public string Email { get; private set; } = null!;
    public Guid UserId { get; private set; }
    
    public Candidate(string firstName, string lastName, int age, string email, Guid userId, DateTimeOffset createdAt)
    {
        var createdEvent = new CandidateCreatedEvent(this,
            new CandidateCreatedEventData(firstName, lastName, age, email, userId), createdAt);

        AddEvent(createdEvent);
    }

    public Candidate(IEnumerable<IDomainEvent<Guid>> domainEvents, CandidateSnapshot? snapshot) 
        : base(domainEvents, snapshot)
    {
    }

    protected override Result ApplyEventEffect(IDomainEvent<Guid> domainEvent)
    {
        return domainEvent switch
        {
            CandidateCreatedEvent createdEvent => ApplyCreatedEvent(createdEvent),
            _ => Result.Fail($"Event {domainEvent.Name} is not supported by {nameof(Candidate)}")
        };
    }

    protected override void RestoreSnapshot(CandidateSnapshot snapshot)
    {
        FirstName = snapshot.FirstName;
        LastName = snapshot.LastName;
        Age = snapshot.Age;
        Email = snapshot.Email;
        UserId = snapshot.UserId;
    }

    private Result ApplyCreatedEvent(CandidateCreatedEvent @event)
    {
        //TODO add validation
        
        FirstName = @event.Data.FirstName;
        LastName = @event.Data.LastName;
        Age = @event.Data.Age;
        Email = @event.Data.Email;
        UserId = @event.Data.UserId;

        return Result.Ok();
    }

    public Result UpdatePersonalData(string firstName, string lastName, int age)
    {
        
    }
}