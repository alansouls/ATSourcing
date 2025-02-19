using ATSourcing.Domain.JobApplications.Events;
using ATSourcing.Domain.StepDefinitions.Snapshots;

namespace ATSourcing.Infrastructure.JobApplications.SerializableEventDatas;

internal class SerializableJobApplicationCreatedEventData
{
    public Guid CandidateId { get; set; }

    public Guid JobId { get; set; }

    public required StepSnapshot CurrentStep { get; set; }

    public static SerializableJobApplicationCreatedEventData FromJobApplicationCreatedEventData(JobApplicationCreatedEventData jobApplicationCreatedEventData)
    {
        return new SerializableJobApplicationCreatedEventData
        {
            CandidateId = jobApplicationCreatedEventData.CandidateId,
            JobId = jobApplicationCreatedEventData.JobId,
            CurrentStep = jobApplicationCreatedEventData.CurrentStep.ToSnapshot()
        };
    }

    public JobApplicationCreatedEventData ToJobApplicationCreatedEventData()
    {
        return new JobApplicationCreatedEventData(
            CandidateId,
            JobId,
            CurrentStep.ToStep().Value
        );
    }
}
