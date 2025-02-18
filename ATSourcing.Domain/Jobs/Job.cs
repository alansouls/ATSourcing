using ATSourcing.Domain.Candidates.Events;
using ATSourcing.Domain.Jobs.Events;
using ATSourcing.Domain.Jobs.Snapshots;
using ATSourcing.Domain.StepDefinitions.Definitions;
using ATSourcing.Domain.ValueObjects;
using ESFrame.Domain;
using ESFrame.Domain.Interfaces;
using FluentResults;

namespace ATSourcing.Domain.Jobs;

public class Job : BaseAggregateRoot<JobSnapshot, Guid>
{
    public string Title { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    private List<Guid> _candidates = null!;
    public IReadOnlyList<Guid> Candidates => _candidates;

    public DateTimeOffset ApplicationDeadline { get; private set; }

    public int VacancyCount { get; private set; }

    public StepFlowDefinition StepFlow { get; private set; } = null!;

    public DecimalRange? SalaryRange { get; private set; }

    public Job(string title, string description, DateTimeOffset applicationDeadline, int vacancyCount, StepFlowDefinition stepFlow, DecimalRange? salaryRange, DateTimeOffset createdAt, Guid? id = null)
    {
        var createdEvent = new JobCreatedEvent(id ?? Guid.NewGuid(),
            new JobCreatedEventData(title, description, applicationDeadline, vacancyCount, salaryRange, stepFlow), createdAt);

        AddEvent(createdEvent);
    }

    public Job(IEnumerable<IDomainEvent<Guid>> events, JobSnapshot? snapshot) : base(events, snapshot)
    {
    }

    public override JobSnapshot CreateSnapshot()
    {
        return new JobSnapshot
        {
            Id = Guid.NewGuid(),
            AggregateId = Id,
            Title = Title,
            Description = Description,
            Candidates = _candidates,
            ApplicationDeadline = ApplicationDeadline,
            VacancyCount = VacancyCount,
            SalaryRange = SalaryRange
        };
    }

    protected override Result ApplyEventEffect(IDomainEvent<Guid> domainEvent)
    {
        if (IsDeleted)
        {
            return Result.Fail($"Entity {nameof(Job)} with id {Id} is deleted and cannot be modified");
        }

        return domainEvent switch
        {
            JobCreatedEvent createdEvent => ApplyCreatedEvent(createdEvent),
            JobUpdatedEvent updatedEvent => ApplyUpdatedEvent(updatedEvent),
            JobDeletedEvent deletedEvent => ApplyDeletedEvent(deletedEvent),
            JobCandidateApplicationAddedEvent candidateApplicationAddedEvent => ApplyAddCandidateApplicationEvent(candidateApplicationAddedEvent),
            JobCandidateApplicationRemovedEvent candidateApplicationRemovedEvent => ApplyRemoveCandidateApplicationEvent(candidateApplicationRemovedEvent),
            _ => Result.Fail($"Event {domainEvent.Name} is not supported by {nameof(Job)}")
        };
    }

    private Result ApplyCreatedEvent(JobCreatedEvent @event)
    {
        Id = @event.AggregateId;
        Title = @event.Data.Title;
        Description = @event.Data.Description;
        ApplicationDeadline = @event.Data.ApplicationDeadline;
        VacancyCount = @event.Data.VacancyCount;
        SalaryRange = @event.Data.SalaryRange;
        _candidates = [];
        return Result.Ok();
    }

    private Result ApplyUpdatedEvent(JobUpdatedEvent @event)
    {
        switch (@event.Data.FieldName)
        {
            case nameof(Title):
                Title = @event.Data.NewValue;
                break;
            case nameof(Description):
                Description = @event.Data.NewValue;
                break;
            case nameof(ApplicationDeadline):
                ApplicationDeadline = DateTimeOffset.Parse(@event.Data.NewValue);
                break;
            case nameof(VacancyCount):
                VacancyCount = int.Parse(@event.Data.NewValue);
                break;
            case nameof(SalaryRange):
                SalaryRange = DecimalRange.Parse(@event.Data.NewValue);
                break;
            default:
                return Result.Fail($"Field {@event.Data.FieldName} is not supported by {nameof(Job)}");
        }

        return Result.Ok();
    }

    private Result ApplyDeletedEvent(JobDeletedEvent @event)
    {
        IsDeleted = true;
        return Result.Ok();
    }

    private Result ApplyAddCandidateApplicationEvent(JobCandidateApplicationAddedEvent @event)
    {
        if (_candidates.Contains(@event.Data.CandidateId))
        {
            return Result.Fail($"Candidate with id {@event.Data.CandidateId} is already applied to job with id {Id}");
        }

        _candidates.Add(@event.Data.CandidateId);
        return Result.Ok();
    }

    private Result ApplyRemoveCandidateApplicationEvent(JobCandidateApplicationRemovedEvent @event)
    {
        if (!_candidates.Contains(@event.Data.CandidateId))
        {
            return Result.Fail($"Candidate with id {@event.Data.CandidateId} is not applied to job with id {Id}");
        }

        _candidates.Remove(@event.Data.CandidateId);
        return Result.Ok();
    }

    protected override void RestoreSnapshot(JobSnapshot snapshot)
    {
        Title = snapshot.Title;
        Description = snapshot.Description;
        _candidates.AddRange(snapshot.Candidates);
        ApplicationDeadline = snapshot.ApplicationDeadline;
        VacancyCount = snapshot.VacancyCount;
        SalaryRange = snapshot.SalaryRange;
    }

    public Result SetTitle(string title, DateTimeOffset updatedAt)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Fail("Title cannot be empty");
        }

        return AddEvent(new JobUpdatedEvent(Id, new JobUpdatedEventData(nameof(Title), title), updatedAt));
    }

    public Result SetDescription(string description, DateTimeOffset updatedAt)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return Result.Fail("Description cannot be empty");
        }
        return AddEvent(new JobUpdatedEvent(Id, new JobUpdatedEventData(nameof(Description), description), updatedAt));
    }

    public Result SetApplicationDeadline(DateTimeOffset? applicationDeadline, DateTimeOffset updatedAt)
    {
        if (applicationDeadline < DateTimeOffset.Now)
        {
            return Result.Fail("Application deadline cannot be in the past");
        }
        return AddEvent(new JobUpdatedEvent(Id, new JobUpdatedEventData(nameof(ApplicationDeadline), applicationDeadline.ToString()), updatedAt));
    }

    public Result SetVacancyCount(int vacancyCount, DateTimeOffset updatedAt)
    {
        if (vacancyCount < 1)
        {
            return Result.Fail("Vacancy count cannot be less than 1");
        }
        return AddEvent(new JobUpdatedEvent(Id, new JobUpdatedEventData(nameof(VacancyCount), vacancyCount.ToString()), updatedAt));
    }

    public Result SetSalaryRange(DecimalRange salaryRange, DateTimeOffset updatedAt)
    {
        return AddEvent(new JobUpdatedEvent(Id, new JobUpdatedEventData(nameof(SalaryRange), salaryRange.ToString()), updatedAt));
    }

    public Result Delete(DateTimeOffset deletedAt)
    {
        return AddEvent(new JobDeletedEvent(Id, deletedAt));
    }

    public Result AddCandidateApplication(Guid candidateId, DateTimeOffset addedAt)
    {
        return AddEvent(new JobCandidateApplicationAddedEvent(Id, new JobCandidateApplicationAddedEventData(candidateId), addedAt));
    }

    public Result RemoveCandidateApplication(Guid candidateId, DateTimeOffset removedAt)
    {
        return AddEvent(new JobCandidateApplicationRemovedEvent(Id, new JobCandidateApplicationRemovedEventData(candidateId), removedAt));
    }
}
