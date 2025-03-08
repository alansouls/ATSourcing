using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.JobApplications.Requests.Commands;

public record ApproveJobApplicationStepCommand(Guid JobApplicationId, string? FinalObservation) : ICommand;
