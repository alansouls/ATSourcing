
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Candidates.Requests.Commands;

public record DeleteCandidateCommand(Guid CandidateId) : ICommand;
