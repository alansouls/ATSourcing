using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.JobApplications.Events;

public record JobApplicationAnswerAddedEventData(bool FromCandidate, string Answer)
{
    public bool FromRecruiter => !FromCandidate;
}

public class JobApplicationAnswerAddedEvent : DomainEvent<Guid, JobApplicationAnswerAddedEventData>
{
    public override string Name => nameof(JobApplicationAnswerAddedEvent);

    public JobApplicationAnswerAddedEvent(Guid jobApplicationId, JobApplicationAnswerAddedEventData data, DateTimeOffset createdAt)
    {
        AggregateId = jobApplicationId;
        Data = data;
        TimeStamp = createdAt;
    }
}
