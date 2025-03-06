using ATSourcing.Domain.Candidates.Events;
using ATSourcing.Domain.Candidates.Snapshots;
using ESFrame.Domain;
using ESFrame.Domain.Interfaces;
using FluentResults;

namespace ATSourcing.Domain.Candidates;

public class Candidate : BaseAggregateRoot<CandidateSnapshot, Guid>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public int Age { get; private set; }
    public string Email { get; private set; } = null!;
    public Guid UserId { get; private set; }
    
    public Candidate(string firstName, string lastName, int age, string email, Guid userId, DateTimeOffset createdAt, Guid? id = null)
    {
        var createdEvent = new CandidateCreatedEvent(id ?? Guid.NewGuid(),
            new CandidateCreatedEventData(firstName, lastName, age, email, userId), createdAt);

        AddEvent(createdEvent);
    }

    public Candidate(IEnumerable<IDomainEvent<Guid>> domainEvents, CandidateSnapshot? snapshot) 
        : base(domainEvents, snapshot)
    {
    }

    protected override Result ApplyEventEffect(IDomainEvent<Guid> domainEvent)
    {
        if (IsDeleted)
        {
            return Result.Fail($"Entity {nameof(Candidate)} with id {Id} is deleted and cannot be modified");
        }
        
        return domainEvent switch
        {
            CandidateCreatedEvent createdEvent => ApplyCreatedEvent(createdEvent),
            CandidateUpdatedEvent updatedEvent => ApplyUpdatedEvent(updatedEvent),
            CandidateDeletedEvent deletedEvent => ApplyDeletedEvent(deletedEvent),
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
        IsDeleted = snapshot.IsDeleted;
    }

    private Result ApplyCreatedEvent(CandidateCreatedEvent @event)
    {
        //TODO add validation

        Id = @event.AggregateId;
        FirstName = @event.Data.FirstName;
        LastName = @event.Data.LastName;
        Age = @event.Data.Age;
        Email = @event.Data.Email;
        UserId = @event.Data.UserId;

        return Result.Ok();
    }

    private Result ApplyUpdatedEvent(CandidateUpdatedEvent updatedEvent)
    {
        if (updatedEvent.Data.FieldName == nameof(FirstName))
        {
            FirstName = @updatedEvent.Data.NewValue;   
        }

        switch (updatedEvent.Data.FieldName)
        {
            case nameof(FirstName):
                FirstName = @updatedEvent.Data.NewValue;
                break;
            case nameof(LastName):
                LastName = @updatedEvent.Data.NewValue;
                break;
            case nameof(Age):
                Age = int.Parse(@updatedEvent.Data.NewValue);
                break;
            case nameof(Email):
                Email = @updatedEvent.Data.NewValue;
                break;
            default:
                return Result.Fail($"Field {updatedEvent.Data.FieldName} is not supported by {nameof(Candidate)}");
        }

        return Result.Ok();
    }

    private Result ApplyDeletedEvent(CandidateDeletedEvent deletedEvent)
    {
        IsDeleted = true;

        return Result.Ok();
    }

    public void UpdateFirstName(string firstName, DateTimeOffset updatedAt)
    {
        if (FirstName == firstName)
        {
            return;
        }
        
        var updatedEvent = new CandidateUpdatedEvent(Id!, new CandidateUpdatedEventData(nameof(FirstName), 
                firstName), updatedAt);

        AddEvent(updatedEvent);
    }
    
    public void UpdateLastName(string lastName, DateTimeOffset updatedAt)
    {
        if (LastName == lastName)
        {
            return;
        }
        
        var updatedEvent = new CandidateUpdatedEvent(Id!, new CandidateUpdatedEventData(nameof(LastName), 
                lastName), updatedAt);

        AddEvent(updatedEvent);
    }
    
    public void UpdateAge(int age, DateTimeOffset updatedAt)
    {
        if (Age == age)
        {
            return;
        }
        
        var updatedEvent = new CandidateUpdatedEvent(Id!, new CandidateUpdatedEventData(nameof(Age), 
                age.ToString()), updatedAt);

        AddEvent(updatedEvent);
    }
    
    public void UpdateEmail(string email, DateTimeOffset updatedAt)
    {
        if (Email == email)
        {
            return;
        }
        
        var updatedEvent = new CandidateUpdatedEvent(Id!, new CandidateUpdatedEventData(nameof(Email), 
                email), updatedAt);

        AddEvent(updatedEvent);
    }

    public Result Delete(DateTimeOffset deletedAt)
    {
        var deletedEvent = new CandidateDeletedEvent(Id, deletedAt);
        
        return AddEvent(deletedEvent);
    }

    public override CandidateSnapshot CreateSnapshot()
    {
        return new CandidateSnapshot
        {
            AggregateId = Id,
            FirstName = FirstName,
            LastName = LastName,
            Age = Age,
            Email = Email,
            UserId = UserId,
            IsDeleted = IsDeleted
        };
    }
}