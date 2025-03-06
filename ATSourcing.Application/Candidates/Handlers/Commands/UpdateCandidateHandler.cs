using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.Candidates.Requests.Commands;
using ATSourcing.Domain.Candidates;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Candidates.Handlers.Commands;

public class UpdateCandidateHandler : IRequestHandler<UpdateCandidateCommand, Result>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly TimeProvider _timeProvider;
    
    public UpdateCandidateHandler(ICandidateRepository candidateRepository, TimeProvider timeProvider)
    {
        _candidateRepository = candidateRepository;
        _timeProvider = timeProvider;
    }
    
    public async Task<Result> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.CandidateId, cancellationToken);

        if (candidate is null)
        {
            return Result.Fail(new NotFoundError(nameof(Candidate), request.CandidateId.ToString()));
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            candidate.UpdateFirstName(request.FirstName, _timeProvider.GetUtcNow());
        }
        
        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            candidate.UpdateLastName(request.LastName, _timeProvider.GetUtcNow());
        }
        
        if (request.Age.HasValue)
        {
            candidate.UpdateAge(request.Age.Value, _timeProvider.GetUtcNow());
        }
        
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            candidate.UpdateEmail(request.Email, _timeProvider.GetUtcNow());
        }
        
        return await _candidateRepository.SaveChangesAsync(candidate, cancellationToken);
    }
}