using ATSourcing.Domain.JobApplications.Events;
using ATSourcing.Domain.StepDefinitions.Snapshots;

namespace ATSourcing.Infrastructure.JobApplications.SerializableEventDatas;

internal class SerializableJobApplicationCurrentStepRejectedEventData
{
    public string? FinalObservation { get; set; }

    public static SerializableJobApplicationCurrentStepRejectedEventData FromJobApplicationCurrentStepRejectedEventData(
        JobApplicationCurrentStepRejectedEventData jobApplicationCurrentStepRejectedEvent)
    {
        return new SerializableJobApplicationCurrentStepRejectedEventData
        {
            FinalObservation = jobApplicationCurrentStepRejectedEvent.FinalObservation
        };
    }

    public JobApplicationCurrentStepRejectedEventData ToJobApplicationCurrentStepRejectedEventData()
    {
        return new JobApplicationCurrentStepRejectedEventData(
            FinalObservation
        );
    }
}
