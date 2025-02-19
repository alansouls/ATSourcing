using ATSourcing.Domain.Jobs.Events;
using ATSourcing.Domain.StepDefinitions;
using ATSourcing.Domain.StepDefinitions.Definitions;
using ATSourcing.Domain.StepDefinitions.Definitions.Snapshots;
using ATSourcing.Domain.StepDefinitions.Enums;
using ATSourcing.Domain.ValueObjects;

namespace ATSourcing.Infrastructure.Jobs.SerializableEventDatas;

public class SerializableJobCreatedEventData
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTimeOffset ApplicationDeadline { get; set; }
    public int VacancyCount { get; set; }
    public DecimalRange? SalaryRange { get; set; }
    public StepFlowDefinitionSnapshot StepFlow { get; set; } = default!;

    public static SerializableJobCreatedEventData FromJobCreatedEventData(JobCreatedEventData jobCreatedEventData)
    {
        return new SerializableJobCreatedEventData
        {
            Title = jobCreatedEventData.Title,
            Description = jobCreatedEventData.Description,
            ApplicationDeadline = jobCreatedEventData.ApplicationDeadline,
            VacancyCount = jobCreatedEventData.VacancyCount,
            SalaryRange = jobCreatedEventData.SalaryRange,
            StepFlow = jobCreatedEventData.StepFlow.ToSnapshot()
        };
    }

    public JobCreatedEventData ToJobCreatedEventData()
    {
        return new JobCreatedEventData(
            Title,
            Description,
            ApplicationDeadline,
            VacancyCount,
            SalaryRange,
            StepFlow.ToStepFlowDefinition()
        );
    }
}