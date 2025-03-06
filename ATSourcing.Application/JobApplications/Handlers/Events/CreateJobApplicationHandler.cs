using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Domain.JobApplications;
using ATSourcing.Domain.Jobs.Events;
using ESFrame.Application;
using MediatR;

namespace ATSourcing.Application.JobApplications.Handlers.Events;

internal class
    CreateJobApplicationHandler : INotificationHandler<ApplicationNotification<JobCandidateApplicationAddedEvent>>
{
    private readonly TimeProvider _timeProvider;
    private readonly IJobRepository _jobRepository;
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public CreateJobApplicationHandler(IJobApplicationRepository jobApplicationRepository, IJobRepository jobRepository,
        TimeProvider timeProvider)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _jobRepository = jobRepository;
        _timeProvider = timeProvider;
    }

    public async Task Handle(ApplicationNotification<JobCandidateApplicationAddedEvent> notification,
        CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(notification.Event.AggregateId, cancellationToken);

        if (job is null)
        {
            return;
        }

        var firstStep = job.StepFlow.Current.CreateStep();

        var jobApplication = new JobApplication(notification.Event.Data.CandidateId, notification.Event.AggregateId,
            firstStep, _timeProvider.GetUtcNow());

        await _jobApplicationRepository.SaveChangesAsync(jobApplication, cancellationToken);
    }
}