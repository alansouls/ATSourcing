using ATSourcing.Domain.StepDefinitions.Enums;

namespace ATSourcing.Domain.StepDefinitions;

public class FileUploadStepDefinition : StepDefinition
{
    public override string Name => "File Upload";

    public override string Description => "Upload the required files";

    public override StepState StartingState => StepState.PendingCandidate;

    public required List<string> RequiredFiles { get; set; }
}
