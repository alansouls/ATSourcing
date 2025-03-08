using ATSourcing.Domain.JobApplications.Events;
using ATSourcing.Domain.JobApplications.Snapshots;
using ATSourcing.Domain.StepDefinitions;
using ATSourcing.Domain.StepDefinitions.Enums;
using ESFrame.Domain;
using ESFrame.Domain.Interfaces;
using FluentResults;

namespace ATSourcing.Domain.JobApplications;

public class JobApplication : BaseAggregateRoot<JobApplicationSnapshot, Guid>
{
    public Guid CandidateId { get; private set; }
    public Guid JobId { get; private set; }

    private readonly List<Step> _stepHistory = [];
    public IReadOnlyList<Step> StepHistory => _stepHistory;
    public Step CurrentStep => StepHistory.Last();
    public int CurrentStepIndex => StepHistory.Count - 1;
    public StepState CurrentState => CurrentStep.State;

    public JobApplication(Guid candidateId, Guid jobId, Step currentStep, DateTimeOffset createdAt)
    {
        AddEvent(new JobApplicationCreatedEvent(Guid.NewGuid(), new JobApplicationCreatedEventData(candidateId, jobId, currentStep), createdAt));
    }

    public JobApplication(IEnumerable<IDomainEvent<Guid>> domainEvents, JobApplicationSnapshot? snapshot)
        : base(domainEvents, snapshot)
    {
    }

    public Result AddCandidateAnswer(string answer, DateTimeOffset answeredDate)
    {
        if (CurrentStep is not ConversationStep conversationStep)
        {
            return Result.Fail($"Current step is not a conversation step");
        }

        if (conversationStep.State != StepState.PendingCandidate)
        {
            return Result.Fail("Cannot add answer to a step that is not pending candidate");
        }

        return AddEvent(new JobApplicationAnswerAddedEvent(Id, new JobApplicationAnswerAddedEventData(FromCandidate: true, answer), answeredDate));
    }

    public Result AddRecruiterAnswer(string answer, DateTimeOffset answeredDate)
    {
        if (CurrentStep is not ConversationStep conversationStep)
        {
            return Result.Fail($"Current step is not a conversation step");
        }

        if (conversationStep.State != StepState.PendingRecruiter)
        {
            return Result.Fail("Cannot add answer to a step that is not pending recruiter");
        }

        return AddEvent(new JobApplicationAnswerAddedEvent(Id, new JobApplicationAnswerAddedEventData(FromCandidate: false, answer), answeredDate));
    }

    public Result ApproveCurrentStep(Step? nextStep, string? finalObservation, DateTimeOffset approvedDate)
    {
        return AddEvent(new JobApplicationCurrentStepApprovedEvent(Id, new JobApplicationCurrentStepApprovedEventData(nextStep, finalObservation), approvedDate));
    }

    public Result RejectCurrentStep(string? finalObservation, DateTimeOffset rejectedDate)
    {
        return AddEvent(new JobApplicationCurrentStepRejectedEvent(Id, new JobApplicationCurrentStepRejectedEventData(finalObservation), rejectedDate));
    }

    public override JobApplicationSnapshot CreateSnapshot()
    {
        return new JobApplicationSnapshot
        {
            Id = Guid.NewGuid(),
            AggregateId = Id,
            TimeStamp = DateTimeOffset.Now,
            CandidateId = CandidateId,
            JobId = JobId,
            StepHistory = _stepHistory.Select(step => step.ToSnapshot()).ToList()
        };
    }

    protected override Result ApplyEventEffect(IDomainEvent<Guid> domainEvent)
    {
        return domainEvent switch
        {
            JobApplicationCreatedEvent createdEvent => ApplyCreatedEvent(createdEvent),
            JobApplicationAnswerAddedEvent answerAddedEvent => ApplyAsnwerAddedEvent(answerAddedEvent),
            JobApplicationCurrentStepApprovedEvent currentStepApprovedEvent => ApplyCurrentStepApprovedEvent(currentStepApprovedEvent),
            JobApplicationCurrentStepRejectedEvent currentStepRejectedEvent => ApplyCurrentStepRejectedEvent(currentStepRejectedEvent),
            _ => Result.Fail($"Event {domainEvent.Name} is not supported by {nameof(JobApplication)}")
        };
    }

    private Result ApplyCreatedEvent(JobApplicationCreatedEvent @event)
    {
        Id = @event.AggregateId;
        CandidateId = @event.Data.CandidateId;
        JobId = @event.Data.JobId;
        _stepHistory.Add(@event.Data.CurrentStep);
        return Result.Ok();
    }

    private Result ApplyAsnwerAddedEvent(JobApplicationAnswerAddedEvent @event)
    {
        if (CurrentStep is not ConversationStep conversationStep)
        {
            return Result.Fail($"Current step is not a conversation step");
        }

        if (@event.Data.FromCandidate && CurrentState != StepState.PendingCandidate)
        {
            return Result.Fail("Cannot add answer to a step that is not pending candidate");
        }

        if (!@event.Data.FromCandidate && CurrentState != StepState.PendingRecruiter)
        {
            return Result.Fail("Cannot add answer to a step that is not pending recruiter");
        }

        return conversationStep.AddAnswer(@event.Data.Answer);
    }

    private Result ApplyCurrentStepApprovedEvent(JobApplicationCurrentStepApprovedEvent @event)
    {
        CurrentStep.Approve(@event.Data.FinalObservation);
        if (@event.Data.NextStep is not null)
        {
            _stepHistory.Add(@event.Data.NextStep);
        }
        return Result.Ok();
    }

    private Result ApplyCurrentStepRejectedEvent(JobApplicationCurrentStepRejectedEvent @event)
    {
        CurrentStep.Reject(@event.Data.FinalObservation);
        return Result.Ok();
    }

    protected override void RestoreSnapshot(JobApplicationSnapshot snapshot)
    {
        CandidateId = snapshot.CandidateId;
        JobId = snapshot.JobId;
        _stepHistory.AddRange(snapshot.StepHistory.Select(s => s.ToStep().Value));
    }
}
