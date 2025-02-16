using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Application.Jobs.Requests.Queries;
using ATSourcing.Application.Jobs.Views;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Jobs.Handlers.Queries;

internal class GetJobHandler : IRequestHandler<GetJobQuery, Result<JobInfoView?>>
{
    private readonly IJobViewService _jobViewService;

    public GetJobHandler(IJobViewService jobViewService)
    {
        _jobViewService = jobViewService;
    }

    public async Task<Result<JobInfoView?>> Handle(GetJobQuery request, 
        CancellationToken cancellationToken)
    {
        var job = await _jobViewService.GetJobInfoById(request.JobId, cancellationToken);

        if (job is null)
        {
            return Result.Fail<JobInfoView?>(new NotFoundError(nameof(job), request.JobId.ToString()));
        }

        return job;
    }
}
