using ATSourcing.Domain.StepDefinitions.Definitions.Snapshots;

namespace ATSourcing.Domain.StepDefinitions.Definitions;

public class StepFlowDefinition
{
    public StepDefinition Current { get; }

    public StepFlowDefinition? Next { get; }

    public StepFlowDefinition(StepDefinition current, StepFlowDefinition? next)
    {
        Current = current;
        Next = next;
    }

    public StepFlowDefinitionSnapshot ToSnapshot()
    {
        return new StepFlowDefinitionSnapshot
        {
            Current = Current.ToSnapshot(),
            Next = Next?.ToSnapshot()
        };
    }
}
