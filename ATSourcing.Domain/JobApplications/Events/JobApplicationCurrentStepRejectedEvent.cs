using ATSourcing.Domain.StepDefinitions;
using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.JobApplications.Events;

public record JobApplicationCurrentStepRejectedEventData(string? FinalObservation);

public class JobApplicationCurrentStepRejectedEvent : DomainEvent<Guid, JobApplicationCurrentStepRejectedEventData>
{
    public override string Name => nameof(JobApplicationCurrentStepRejectedEvent);
    public JobApplicationCurrentStepRejectedEvent(Guid jobApplicationId, JobApplicationCurrentStepRejectedEventData data, DateTimeOffset createdAt)
    {
        AggregateId = jobApplicationId;
        Data = data;
        TimeStamp = createdAt;
    }
}
