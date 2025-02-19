using ATSourcing.Domain.JobApplications.Events;
using ATSourcing.Domain.StepDefinitions.Snapshots;

namespace ATSourcing.Infrastructure.JobApplications.SerializableEventDatas;

internal class SerializableJobApplicationCurrentStepApprovedEventData
{
    public StepSnapshot? NextStep { get; set; }

    public string? FinalObservation { get; set; }

    public static SerializableJobApplicationCurrentStepApprovedEventData FromJobApplicationCurrentStepApprovedEventData(
        JobApplicationCurrentStepApprovedEventData jobApplicationCurrentStepApprovedEvent)
    {
        return new SerializableJobApplicationCurrentStepApprovedEventData
        {
            NextStep = jobApplicationCurrentStepApprovedEvent.NextStep?.ToSnapshot(),
            FinalObservation = jobApplicationCurrentStepApprovedEvent.FinalObservation
        };
    }

    public JobApplicationCurrentStepApprovedEventData ToJobApplicationCurrentStepApprovedEventData()
    {
        return new JobApplicationCurrentStepApprovedEventData(
            NextStep?.ToStep().Value,
            FinalObservation
        );
    }
}
