using ATSourcing.Domain.StepDefinitions.Definitions;
using ATSourcing.Domain.StepDefinitions.Enums;

namespace ATSourcing.Domain.StepDefinitions.Definitions.Snapshots;

public class StepFlowDefinitionSnapshot
{
    public StepDefinitionSnapshot Current { get; set; } = default!;

    public StepFlowDefinitionSnapshot? Next { get; set; }

    public StepFlowDefinition ToStepFlowDefinition()
    {
        return new StepFlowDefinition(Current.ToStepDefinition(), Next?.ToStepFlowDefinition());
    }
}
