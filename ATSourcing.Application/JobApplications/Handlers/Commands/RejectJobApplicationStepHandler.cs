using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Application.JobApplications.Requests.Commands;
using ATSourcing.Application.Jobs.Interfaces;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.JobApplications.Handlers.Commands;

internal class RejectJobApplicationStepHandler : IRequestHandler<RejectJobApplicationStepCommand, Result>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IJobRepository _jobRepository;
    private readonly TimeProvider _timeProvider;

    public RejectJobApplicationStepHandler(IJobApplicationRepository jobApplicationRepository, IJobRepository jobRepository, TimeProvider timeProvider)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _jobRepository = jobRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(RejectJobApplicationStepCommand request, CancellationToken cancellationToken)
    {
        var jobApplication = await _jobApplicationRepository.GetByIdAsync(request.JobApplicationId, cancellationToken);

        if (jobApplication == null)
        {
            return Result.Fail(new NotFoundError(nameof(jobApplication), request.JobApplicationId.ToString()));
        }

        var job = await _jobRepository.GetByIdAsync(jobApplication.JobId, cancellationToken);

        if (job == null)
        {
            throw new Exception($"Job with id {jobApplication.JobId} not found");
        }

        var nextStep = job.GetNextStepDefinition(jobApplication.CurrentStepIndex);

        var result = jobApplication.RejectCurrentStep(request.FinalObservation, _timeProvider.GetUtcNow());

        if (result.IsFailed)
        {
            return result;
        }

        return await _jobApplicationRepository.SaveChangesAsync(jobApplication, cancellationToken);
    }
}
