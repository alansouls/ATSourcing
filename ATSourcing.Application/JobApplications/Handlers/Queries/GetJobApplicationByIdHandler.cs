using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Application.JobApplications.Requests.Queries;
using ATSourcing.Application.JobApplications.Views;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.JobApplications.Handlers.Queries;

internal class GetJobApplicationByIdHandler : IRequestHandler<GetJobApplicationByIdQuery, Result<JobApplicationInfoView>>
{
    private readonly IJobApplicationViewService _jobApplicationViewService;

    public GetJobApplicationByIdHandler(IJobApplicationViewService jobApplicationViewService)
    {
        _jobApplicationViewService = jobApplicationViewService;
    }

    public async Task<Result<JobApplicationInfoView>> Handle(GetJobApplicationByIdQuery request, CancellationToken cancellationToken)
    {
        var jobApplication = await _jobApplicationViewService.GetJobApplicationInfoViewById(request.JobApplicationId, cancellationToken);

        if (jobApplication is null)
        {
            return Result.Fail(new NotFoundError(nameof(jobApplication), request.JobApplicationId.ToString()));
        }

        return jobApplication;
    }
}
