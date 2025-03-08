using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Application.JobApplications.Views;
using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Domain.JobApplications.Events;
using ESFrame.Application;
using MediatR;

namespace ATSourcing.Application.Jobs.Handlers.Events;

internal class JobApplicationItemViewUpdaterHandler : INotificationHandler<ApplicationNotification<JobApplicationCreatedEvent>>,
    INotificationHandler<ApplicationNotification<JobApplicationAnswerAddedEvent>>,
    INotificationHandler<ApplicationNotification<JobApplicationCurrentStepApprovedEvent>>,
    INotificationHandler<ApplicationNotification<JobApplicationCurrentStepRejectedEvent>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ICandidateViewService _candidateViewService;
    private readonly IJobApplicationViewService _jobApplicationViewService;

    public JobApplicationItemViewUpdaterHandler(IJobApplicationViewService jobApplicationViewService, ICandidateViewService candidateViewService,
        IJobRepository jobRepository, IJobApplicationRepository jobApplicationRepository)
    {
        _jobApplicationViewService = jobApplicationViewService;
        _candidateViewService = candidateViewService;
        _jobRepository = jobRepository;
        _jobApplicationRepository = jobApplicationRepository;
    }

    private async Task<JobApplicationItemView?> BuildJobApplicationItemView(Guid jobApplicationId, CancellationToken cancellationToken)
    {
        var jobApplication = await _jobApplicationRepository.GetByIdAsync(jobApplicationId, cancellationToken);

        if (jobApplication is null)
        {
            return null;
        }

        var candidate = await _candidateViewService.GetCandidateInfoById(jobApplication.CandidateId, cancellationToken);
        var job = await _jobRepository.GetByIdAsync(jobApplication.JobId, cancellationToken);

        if (candidate is null || job is null)
        {
            return null;
        }

        return new JobApplicationItemView
        {
            JobApplicationId = jobApplicationId,
            JobId = job.Id,
            JobTitle = job.Title,
            CandidateId = candidate.Id,
            CandidateName = $"{candidate.FirstName} {candidate.LastName}",
            CurrentState = jobApplication.CurrentState,
            CurrentStepTitle = jobApplication.CurrentStep.Title
        };
    }

    public async Task Handle(ApplicationNotification<JobApplicationCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var jobApplicationItemView = await BuildJobApplicationItemView(notification.Event.AggregateId, cancellationToken);

        if (jobApplicationItemView is null)
        {
            return;
        }

        await _jobApplicationViewService.InsertJobApplicationItemViewAsync(jobApplicationItemView, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<JobApplicationAnswerAddedEvent> notification, CancellationToken cancellationToken)
    {
        var jobApplicationItemView = await BuildJobApplicationItemView(notification.Event.AggregateId, cancellationToken);

        if (jobApplicationItemView is null)
        {
            return;
        }

        await _jobApplicationViewService.UpdateJobApplicationItemViewAsync(jobApplicationItemView, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<JobApplicationCurrentStepApprovedEvent> notification, CancellationToken cancellationToken)
    {
        var jobApplicationItemView = await BuildJobApplicationItemView(notification.Event.AggregateId, cancellationToken);

        if (jobApplicationItemView is null)
        {
            return;
        }

        await _jobApplicationViewService.UpdateJobApplicationItemViewAsync(jobApplicationItemView, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<JobApplicationCurrentStepRejectedEvent> notification, CancellationToken cancellationToken)
    {
        var jobApplicationItemView = await BuildJobApplicationItemView(notification.Event.AggregateId, cancellationToken);

        if (jobApplicationItemView is null)
        {
            return;
        }

        await _jobApplicationViewService.UpdateJobApplicationItemViewAsync(jobApplicationItemView, cancellationToken);
    }
}
