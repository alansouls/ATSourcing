using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Candidates.Requests.Commands;

public record UpdateCandidateCommand(Guid CandidateId,
    string? FirstName, 
    string? LastName, 
    int? Age, 
    string? Email) : ICommand;