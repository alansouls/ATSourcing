using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Application.JobApplications.Requests.Queries;
using ATSourcing.Application.JobApplications.Views;
using ESFrame.Application.Views;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.JobApplications.Handlers.Queries;

public class GetJobApplicationsByCandidateIdHandler : IRequestHandler<GetJobApplicationsByCandidateIdQuery,
    Result<IEnumerable<JobApplicationInfoView>>>
{
    private readonly IJobApplicationViewService _jobApplicationViewService;

    public GetJobApplicationsByCandidateIdHandler(IJobApplicationViewService jobApplicationViewService)
    {
        _jobApplicationViewService = jobApplicationViewService;
    }

    public async Task<Result<IEnumerable<JobApplicationInfoView>>> Handle(GetJobApplicationsByCandidateIdQuery request,
        CancellationToken cancellationToken)
    {
        var pagingParameters = ViewPagingParameters.Create(request.Page, request.PageSize, request.Sort,
            request.SortDirection,
            ["id", "jobTitle", "candidateName", "currentState", "currentStepTitle"], 1000);

        if (pagingParameters.IsFailed)
        {
            return pagingParameters.ToResult();
        }

        return await
            _jobApplicationViewService.GetJobApplicationsInfo(request.CandidateId, jobId: null, pagingParameters.Value,
                cancellationToken);
    }
}