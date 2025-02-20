using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Jobs.Requests.Commands;

public record AddCandidateApplicationCommand(Guid JobId, Guid CandidateId) : ICommand;