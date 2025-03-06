using ATSourcing.Domain.StepDefinitions;
using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.JobApplications.Events;

public record JobApplicationCurrentStepApprovedEventData(Step? NextStep, string? FinalObservation);

public class JobApplicationCurrentStepApprovedEvent : DomainEvent<Guid, JobApplicationCurrentStepApprovedEventData>
{
    public override string Name => nameof(JobApplicationCurrentStepApprovedEvent);

    public JobApplicationCurrentStepApprovedEvent(Guid jobApplicationId, JobApplicationCurrentStepApprovedEventData data, DateTimeOffset createdAt)
    {
        AggregateId = jobApplicationId;
        Data = data;
        TimeStamp = createdAt;
    }
}
