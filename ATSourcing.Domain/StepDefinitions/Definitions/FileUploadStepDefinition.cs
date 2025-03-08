using ATSourcing.Domain.StepDefinitions.Definitions.Snapshots;
using ATSourcing.Domain.StepDefinitions.Enums;
using System.Text.Json;

namespace ATSourcing.Domain.StepDefinitions;

public class FileUploadStepDefinition : StepDefinition
{
    public const string StepName = "File Upload";
    public override string Name => StepName;

    public override string Description => "Upload the required files";

    public override StepState StartingState => StepState.PendingCandidate;

    public required List<string> RequiredFiles { get; set; }

    public override string Title => $"Upload the following files {string.Join(", ", RequiredFiles)}";

    public override Step CreateStep()
    {
        return FileUploadStep.Create(RequiredFiles).Value;
    }

    public override StepDefinitionSnapshot ToSnapshot()
    {
        return new StepDefinitionSnapshot
        {
            Name = Name,
            Description = Description,
            StartingState = StartingState,
            SpecificData = new Dictionary<string, string>
            {
                { nameof(RequiredFiles), JsonSerializer.Serialize(RequiredFiles) }
            }
        };
    }
}
