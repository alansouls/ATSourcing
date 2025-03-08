using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Application.JobApplications.Requests.Queries;
using ATSourcing.Application.JobApplications.Views;
using ESFrame.Application.Views;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.JobApplications.Handlers.Queries;

internal class GetJobApplicationsByJobIdHandler : IRequestHandler<GetJobApplicationsByJobIdQuery, Result<ViewPagingResult<JobApplicationItemView>>>
{
    private readonly IJobApplicationViewService _jobApplicationViewService;

    public GetJobApplicationsByJobIdHandler(IJobApplicationViewService jobApplicationViewService)
    {
        _jobApplicationViewService = jobApplicationViewService;
    }

    public async Task<Result<ViewPagingResult<JobApplicationItemView>>> Handle(GetJobApplicationsByJobIdQuery request, CancellationToken cancellationToken)
    {
        var pagingParameters = ViewPagingParameters.Create(request.Page, request.PageSize, request.Sort,
            request.SortDirection,
            ["id", "jobTitle", "candidateName", "currentState", "currentStepTitle"], 1000);

        if (pagingParameters.IsFailed)
        {
            return pagingParameters.ToResult();
        }

        return await
            _jobApplicationViewService.GetJobApplicationItems(candidateId: null, jobId: request.JobId, pagingParameters.Value,
                cancellationToken);
    }
}
