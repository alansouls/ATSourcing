using ATSourcing.Domain.StepDefinitions;
using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.JobApplications.Events;

public record JobApplicationCreatedEventData(Guid CandidateId, Guid JobId, Step CurrentStep);

public class JobApplicationCreatedEvent : DomainEvent<Guid, JobApplicationCreatedEventData>
{
    public JobApplicationCreatedEvent(Guid jobApplicationId, JobApplicationCreatedEventData data, DateTimeOffset createdAt)
    {
        AggregateId = jobApplicationId;
        Data = data;
        TimeStamp = createdAt;
    }

    public override string Name => nameof(JobApplicationCreatedEvent);
}
