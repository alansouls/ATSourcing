using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Application.Jobs.Requests.Queries;
using ATSourcing.Application.Jobs.Views;
using ESFrame.Application.Views;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Jobs.Handlers.Queries;

internal class GetJobsHandler : IRequestHandler<GetJobsQuery, Result<ViewPagingResult<JobInfoView>>>
{
    private readonly IJobViewService _jobViewService;

    public GetJobsHandler(IJobViewService jobViewService)
    {
        _jobViewService = jobViewService;
    }

    public async Task<Result<ViewPagingResult<JobInfoView>>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        var parameters = ViewPagingParameters.Create(request.Page, request.PageSize, request.Sort, request.SortDirection,
            ["id", "title", "description", "vacancyCount", "applicationDeadline", "salaryRange.min", "salaryRange.max"], 1000);

        if (parameters.IsFailed)
        {
            return parameters.ToResult();
        }

        return await _jobViewService.GetJobsInfo(parameters.Value, cancellationToken);
    }
}
