using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.JobApplications.Requests.Commands;

public record AnswerJobApplicationCommand(Guid JobApplicationId, string Answer) : ICommand;