using ATSourcing.Domain.StepDefinitions.Enums;

namespace ATSourcing.Domain.StepDefinitions;

public class ConversationStepDefinition : StepDefinition
{
    public override string Name => "Conversation";

    public override string Description => "Conversation with the candidate";

    public override StepState StartingState => StepState.PendingCandidate;

    public required string Question { get; set; }
}
