using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Candidates.Requests.Commands;

public record CreateCandidateCommand(string FirstName, string LastName, int Age, string Email, Guid UserId) : ICommand<Guid>;
