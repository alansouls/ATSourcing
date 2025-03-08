using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.JobApplications.Requests.Commands;

public record RejectJobApplicationStepCommand(Guid JobApplicationId, string? FinalObservation) : ICommand;
