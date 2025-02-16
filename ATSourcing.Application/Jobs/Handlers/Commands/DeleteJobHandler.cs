using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Application.Jobs.Requests.Commands;
using ATSourcing.Domain.Jobs;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Jobs.Handlers.Commands;

internal class DeleteJobHandler : IRequestHandler<DeleteJobCommand, Result>
{
    private readonly IJobRepository _jobRepository;
    private readonly TimeProvider _timeProvider;

    public DeleteJobHandler(IJobRepository jobRepository, TimeProvider timeProvider)
    {
        _jobRepository = jobRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(request.JobId, cancellationToken);

        if (job is null)
        {
            return Result.Fail(new NotFoundError(nameof(Job), request.JobId.ToString()));
        }

        var result = job.Delete(_timeProvider.GetUtcNow());

        if (result.IsFailed)
        {
            return result;
        }

        return await _jobRepository.SaveChangesAsync(job, cancellationToken);
    }
}
