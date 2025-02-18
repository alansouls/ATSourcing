using ATSourcing.Domain.StepDefinitions.Enums;

namespace ATSourcing.Domain.StepDefinitions;

public abstract class StepDefinition
{
    public abstract string Name { get; }
    
    public abstract string Description { get; }

    public abstract StepState StartingState { get; }
}
