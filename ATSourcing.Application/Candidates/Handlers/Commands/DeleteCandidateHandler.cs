using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.Candidates.Requests.Commands;
using ATSourcing.Domain.Candidates;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Candidates.Handlers.Commands;

internal class DeleteCandidateHandler : IRequestHandler<DeleteCandidateCommand, Result>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly TimeProvider _timeProvider;

    public DeleteCandidateHandler(ICandidateRepository candidateRepository, TimeProvider timeProvider)
    {
        _candidateRepository = candidateRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.CandidateId, cancellationToken);

        if (candidate is null)
        {
            return Result.Fail(new NotFoundError(nameof(Candidate), request.CandidateId.ToString()));
        }

        var result = candidate.Delete(_timeProvider.GetUtcNow());

        if (result.IsFailed)
        {
            return result;
        }

        return await _candidateRepository.SaveChangesAsync(candidate, cancellationToken);
    }
}
