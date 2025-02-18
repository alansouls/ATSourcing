using ATSourcing.Domain.Jobs.Events;
using ATSourcing.Domain.StepDefinitions;
using ATSourcing.Domain.StepDefinitions.Definitions;
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
    public SerializableStepFlowDefinition StepFlow { get; set; } = default!;

    public static SerializableJobCreatedEventData FromJobCreatedEventData(JobCreatedEventData jobCreatedEventData)
    {
        return new SerializableJobCreatedEventData
        {
            Title = jobCreatedEventData.Title,
            Description = jobCreatedEventData.Description,
            ApplicationDeadline = jobCreatedEventData.ApplicationDeadline,
            VacancyCount = jobCreatedEventData.VacancyCount,
            SalaryRange = jobCreatedEventData.SalaryRange,
            StepFlow = SerializableStepFlowDefinition.FromStepFlowDefinition(jobCreatedEventData.StepFlow)
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

public class SerializableStepFlowDefinition
{
    public SerializableStepDefinition Current { get; set; } = default!;

    public SerializableStepFlowDefinition? Next { get; set; }

    public static SerializableStepFlowDefinition FromStepFlowDefinition(StepFlowDefinition stepFlowDefinition)
    {
        return new SerializableStepFlowDefinition
        {
            Current = SerializableStepDefinition.FromStepDefinition(stepFlowDefinition.Current),
            Next = stepFlowDefinition.Next is not null ? FromStepFlowDefinition(stepFlowDefinition.Next) : null
        };
    }

    public StepFlowDefinition ToStepFlowDefinition()
    {
        return new StepFlowDefinition(Current.ToStepDefinition(), Next?.ToStepFlowDefinition());
    }
}

public class SerializableStepDefinition
{
    public required string TypeName { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required StepState StartingState { get; set; }

    public required Dictionary<string, string> SpecificData { get; set; }

    public static SerializableStepDefinition FromStepDefinition(StepDefinition stepDefinition)
    {
        return new SerializableStepDefinition
        {
            TypeName = stepDefinition.GetType().Name,
            Name = stepDefinition.Name,
            Description = stepDefinition.Description,
            StartingState = stepDefinition.StartingState,
            SpecificData = stepDefinition switch
            {
                FileUploadStepDefinition fileUploadStepDefinition => new Dictionary<string, string>
                {
                    { "requiredFiles", string.Join("|", fileUploadStepDefinition.RequiredFiles) }
                },
                ConversationStepDefinition conversationStepDefinition => new Dictionary<string, string>
                {
                    { "question", conversationStepDefinition.Question }
                },
                _ => []
            }
        };
    }

    public StepDefinition ToStepDefinition()
    {
        return TypeName switch
        {
            nameof(FileUploadStepDefinition) => new FileUploadStepDefinition
            {
                RequiredFiles = [.. SpecificData["requiredFiles"].Split("|")]
            },
            nameof(ConversationStepDefinition) => new ConversationStepDefinition
            {
                Question = SpecificData["question"]
            },
            _ => throw new NotSupportedException($"StepDefinition type {TypeName} is not supported")
        };
    }
}
