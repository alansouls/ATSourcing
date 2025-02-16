using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Application.Jobs.Requests.Commands;
using ATSourcing.Domain.Jobs;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Jobs.Handlers.Commands;

public class UpdateJobHandler : IRequestHandler<UpdateJobCommand, Result>
{
    private readonly IJobRepository _jobRepository;
    private readonly TimeProvider _timeProvider;
    
    public UpdateJobHandler(IJobRepository jobRepository, TimeProvider timeProvider)
    {
        _jobRepository = jobRepository;
        _timeProvider = timeProvider;
    }
    
    public async Task<Result> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(request.JobId, cancellationToken);

        if (job is null)
        {
            return Result.Fail(new NotFoundError(nameof(Job), request.JobId.ToString()));
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            job.SetTitle(request.Title, _timeProvider.GetUtcNow());
        }
        
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            job.SetDescription(request.Description, _timeProvider.GetUtcNow());
        }
        
        if (request.VacancyCount.HasValue)
        {
            job.SetVacancyCount(request.VacancyCount.Value, _timeProvider.GetUtcNow());
        }
        
        if (request.ApplicationDeadline.HasValue)
        {
            job.SetApplicationDeadline(request.ApplicationDeadline.Value, _timeProvider.GetUtcNow());
        }

        if (request.SalaryRange.IsUpdated)
        {
            job.SetSalaryRange(request.SalaryRange.Value, _timeProvider.GetUtcNow());
        }

        return await _jobRepository.SaveChangesAsync(job, cancellationToken);
    }
}