using ATSourcing.Domain.StepDefinitions.Enums;
using ATSourcing.Domain.StepDefinitions.Snapshots;

namespace ATSourcing.Domain.StepDefinitions;

public abstract class Step
{
    public abstract string Title { get; }
    public abstract string Name { get; }
    
    public abstract string Description { get; }

    public StepState State { get; protected set; }

    public string? FinalObservations { get; protected set; }

    public void Approve(string? finalObservation)
    {
        State = StepState.Approved;
        FinalObservations = finalObservation;
    }

    public void Reject(string? finalObservation)
    {
        State = StepState.Rejected;
        FinalObservations = finalObservation;
    }

    public abstract StepSnapshot ToSnapshot();
}
