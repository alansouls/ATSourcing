using ATSourcing.Domain.StepDefinitions;
using ESFrame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
