using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.Candidates.Requests.Commands;
using ATSourcing.Domain.Candidates;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Candidates.Handlers.Commands;

internal class CreateCandidateHandler : IRequestHandler<CreateCandidateCommand, Result<Guid>>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly TimeProvider _timeProvider;

    public CreateCandidateHandler(ICandidateRepository candidateRepository, TimeProvider timeProvider)
    {
        _candidateRepository = candidateRepository;
        _timeProvider = timeProvider;
    }
    public async Task<Result<Guid>> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = new Candidate(request.FirstName,
            request.LastName, 
            request.Age, request.Email, 
            Guid.NewGuid(), 
            _timeProvider.GetUtcNow());

        var result = await _candidateRepository.SaveChangesAsync(candidate, cancellationToken);

        if (result.IsFailed)
            return result;

        return candidate.Id;
    }
}
