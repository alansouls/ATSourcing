using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Application.Jobs.Requests.Commands;
using ATSourcing.Domain.Jobs;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Jobs.Handlers.Commands;

internal class CreateJobHandler : IRequestHandler<CreateJobCommand, Result<Guid>>
{
    private readonly IJobRepository _jobRepository;
    private readonly TimeProvider _timeProvider;

    public CreateJobHandler(IJobRepository jobRepository, TimeProvider timeProvider)
    {
        _jobRepository = jobRepository;
        _timeProvider = timeProvider;
    }
    public async Task<Result<Guid>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = new Job(request.Title,
            request.Description, 
            request.ApplicationDeadline, 
            request.VacancyCount,
            request.StepFlowDefinition.ToStepFlowDefinition(),
            request.SalaryRange,
            _timeProvider.GetUtcNow());

        var result = await _jobRepository.SaveChangesAsync(job, cancellationToken);

        if (result.IsFailed)
            return result;

        return job.Id;
    }
}
