using ATSourcing.Domain.StepDefinitions.Definitions.Snapshots;
using ATSourcing.Domain.StepDefinitions.Enums;

namespace ATSourcing.Domain.StepDefinitions;

public class ConversationStepDefinition : StepDefinition
{
    public const string StepName = "Conversation";
    public override string Name => StepName;

    public override string Description => "Conversation with the candidate";

    public override StepState StartingState => StepState.PendingCandidate;

    public required string Question { get; set; }

    public override string Title => Question;

    public override Step CreateStep()
    {
        return ConversationStep.Create(Question).Value;
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
                { nameof(Question), Question }
            }
        };
    }
}
