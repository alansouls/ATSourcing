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
}
