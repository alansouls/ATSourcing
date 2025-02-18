using ATSourcing.Domain.StepDefinitions.Enums;
using ATSourcing.Domain.StepDefinitions.Snapshots;

namespace ATSourcing.Domain.StepDefinitions;

public abstract class Step
{
    public abstract string Name { get; }
    
    public abstract string Description { get; }

    public StepState State { get; protected set; }

    public string? FinalObservation { get; private set; }

    public void Approve(string? finalObservation)
    {
        State = StepState.Approved;
        FinalObservation = finalObservation;
    }

    public void Reject(string? finalObservation)
    {
        State = StepState.Rejected;
        FinalObservation = finalObservation;
    }

    public abstract StepSnapshot ToSnapshot();
}
