using ATSourcing.Domain.StepDefinitions.Enums;
using System.Text.Json;

namespace ATSourcing.Domain.StepDefinitions.Definitions.Snapshots;

public class StepDefinitionSnapshot
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required StepState StartingState { get; set; }

    public required Dictionary<string, string> SpecificData { get; set; }

    public StepDefinition ToStepDefinition()
    {
        return Name switch
        {
            FileUploadStepDefinition.StepName => new FileUploadStepDefinition
            {
                RequiredFiles = [.. JsonSerializer.Deserialize<List<string>>(SpecificData[nameof(FileUploadStepDefinition.RequiredFiles)])]
            },
            ConversationStepDefinition.StepName => new ConversationStepDefinition
            {
                Question = SpecificData[nameof(ConversationStepDefinition.Question)]
            },
            _ => throw new NotSupportedException($"StepDefinition {Name} is not supported")
        };
    }
}
