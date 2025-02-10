using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.Candidates.Requests.Queries;
using ATSourcing.Application.Candidates.Views;
using ESFrame.Application.Views;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Candidates.Handlers.Queries;

internal class GetCandidatesHandler : IRequestHandler<GetCandidatesQuery, Result<ViewPagingResult<CandidateInfoView>>>
{
    private readonly ICandidateViewService _candidateViewService;

    public GetCandidatesHandler(ICandidateViewService candidateViewService)
    {
        _candidateViewService = candidateViewService;
    }

    public async Task<Result<ViewPagingResult<CandidateInfoView>>> Handle(GetCandidatesQuery request, CancellationToken cancellationToken)
    {
        var parameters = ViewPagingParameters.Create(request.Page, request.PageSize, request.Sort, request.SortDirection,
            ["id", "firstName", "lastName", "email", "age"], 1000);

        if (parameters.IsFailed)
        {
            return parameters.ToResult();
        }

        return await _candidateViewService.GetCandidatesInfo(parameters.Value, cancellationToken);
    }
}
