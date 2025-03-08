using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Application.JobApplications.Views;
using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Domain.JobApplications;
using ATSourcing.Domain.JobApplications.Events;
using ATSourcing.Domain.StepDefinitions;
using ATSourcing.Domain.ValueObjects;
using ESFrame.Application;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ATSourcing.Application.JobApplications.Handlers.Events;

internal class JobApplicationInfoViewUpdaterHandler : INotificationHandler<ApplicationNotification<JobApplicationCreatedEvent>>,
    INotificationHandler<ApplicationNotification<JobApplicationAnswerAddedEvent>>,
    INotificationHandler<ApplicationNotification<JobApplicationCurrentStepApprovedEvent>>,
    INotificationHandler<ApplicationNotification<JobApplicationCurrentStepRejectedEvent>>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ICandidateViewService _candidateViewService;
    private readonly IJobApplicationViewService _jobApplicationViewService;

    public JobApplicationInfoViewUpdaterHandler(IJobApplicationViewService jobApplicationViewService, ICandidateViewService candidateViewService, 
        IJobRepository jobRepository, IJobApplicationRepository jobApplicationRepository)
    {
        _jobApplicationViewService = jobApplicationViewService;
        _candidateViewService = candidateViewService;
        _jobRepository = jobRepository;
        _jobApplicationRepository = jobApplicationRepository;
    }

    private async Task<JobApplicationInfoView?> BuildJobApplicationInfoView(Guid jobApplicationId, CancellationToken cancellationToken)
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

        var nextStep = job.GetNextStepDefinition(jobApplication.CurrentStepIndex);

        return new JobApplicationInfoView
        {
            JobApplicationId = jobApplicationId,
            JobId = job.Id,
            CandidateId = candidate.Id,
            CurrentStep = jobApplication.CurrentStep.ToSnapshot(),
            CurrentState = jobApplication.CurrentState,
            CandidateName = $"{candidate.FirstName} {candidate.LastName}",
            JobTitle = job.Title,
            JobDescription = job.Description,
            NextStepTitle = nextStep?.Title ?? string.Empty
        };
    }

    public async Task Handle(ApplicationNotification<JobApplicationCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var jobApplicationInfoView = await BuildJobApplicationInfoView(notification.Event.AggregateId, cancellationToken);

        if (jobApplicationInfoView is null)
        {
            return;
        }

        await _jobApplicationViewService.InsertJobApplicationInfoViewAsync(jobApplicationInfoView, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<JobApplicationAnswerAddedEvent> notification, CancellationToken cancellationToken)
    {
        var jobApplicationInfoView = await BuildJobApplicationInfoView(notification.Event.AggregateId, cancellationToken);

        if (jobApplicationInfoView is null)
        {
            return;
        }

        await _jobApplicationViewService.UpdateJobApplicationInfoViewAsync(jobApplicationInfoView, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<JobApplicationCurrentStepApprovedEvent> notification, CancellationToken cancellationToken)
    {
        var jobApplicationInfoView = await BuildJobApplicationInfoView(notification.Event.AggregateId, cancellationToken);

        if (jobApplicationInfoView is null)
        {
            return;
        }

        await _jobApplicationViewService.UpdateJobApplicationInfoViewAsync(jobApplicationInfoView, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<JobApplicationCurrentStepRejectedEvent> notification, CancellationToken cancellationToken)
    {
        var jobApplicationInfoView = await BuildJobApplicationInfoView(notification.Event.AggregateId, cancellationToken);

        if (jobApplicationInfoView is null)
        {
            return;
        }

        await _jobApplicationViewService.UpdateJobApplicationInfoViewAsync(jobApplicationInfoView, cancellationToken);
    }
}
