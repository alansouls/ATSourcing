using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.Candidates.Requests.Queries;
using ATSourcing.Application.Candidates.Views;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Candidates.Handlers.Queries;

internal class GetCandidateHandler : IRequestHandler<GetCandidateQuery, Result<CandidateInfoView?>>
{
    private readonly ICandidateViewService _candidateViewService;

    public GetCandidateHandler(ICandidateViewService candidateViewService)
    {
        _candidateViewService = candidateViewService;
    }

    public async Task<Result<CandidateInfoView?>> Handle(GetCandidateQuery request, 
        CancellationToken cancellationToken)
    {
        var candidate = await _candidateViewService.GetCandidateInfoById(request.CandidateId, cancellationToken);

        if (candidate is null)
        {
            return Result.Fail<CandidateInfoView?>(new NotFoundError(nameof(candidate), request.CandidateId.ToString()));
        }

        return candidate;
    }
}
