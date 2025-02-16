using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Application.Jobs.Views;
using ATSourcing.Domain.Jobs;
using ATSourcing.Domain.Jobs.Events;
using ATSourcing.Domain.ValueObjects;
using ESFrame.Application;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ATSourcing.Application.Jobs.Handlers.Events;

internal class JobInfoViewUpdaterHandler : INotificationHandler<ApplicationNotification<JobCreatedEvent>>,
    INotificationHandler<ApplicationNotification<JobUpdatedEvent>>,
    INotificationHandler<ApplicationNotification<JobDeletedEvent>>
{
    private readonly IJobViewService _jobViewService;
    private readonly ILogger<JobInfoViewUpdaterHandler> _logger;

    public JobInfoViewUpdaterHandler(IJobViewService jobViewService, ILogger<JobInfoViewUpdaterHandler> logger)
    {
        _jobViewService = jobViewService;
        _logger = logger;
    }

    public async Task Handle(ApplicationNotification<JobCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var jobInfoView = new JobInfoView
        {
            JobId = notification.Event.AggregateId,
            Title = notification.Event.Data.Title,
            Description = notification.Event.Data.Description,
            VacancyCount = notification.Event.Data.VacancyCount,
            ApplicationDeadline = notification.Event.Data.ApplicationDeadline,
            SalaryRange = notification.Event.Data.SalaryRange,
            Candidates = []
        };

        await _jobViewService.InsertJobInfoViewAsync(jobInfoView, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<JobUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        var jobInfo = await _jobViewService.GetJobInfoById(notification.Event.AggregateId, cancellationToken);

        if (jobInfo is null)
        {
            _logger.LogWarning("Job with id {JobId} not found", notification.Event.AggregateId);
            return;
        }

        switch (notification.Event.Data.FieldName)
        {
            case nameof(Job.Title):
                jobInfo.Title = notification.Event.Data.NewValue;
                break;
            case nameof(Job.Description):
                jobInfo.Description = notification.Event.Data.NewValue;
                break;
            case nameof(Job.ApplicationDeadline):
                jobInfo.ApplicationDeadline = DateTimeOffset.Parse(notification.Event.Data.NewValue);
                break;
            case nameof(Job.VacancyCount):
                jobInfo.VacancyCount = int.Parse(notification.Event.Data.NewValue);
                break;
            case nameof(Job.SalaryRange):
                jobInfo.SalaryRange = string.IsNullOrEmpty(notification.Event.Data.NewValue) ? null :
                    DecimalRange.Parse(notification.Event.Data.NewValue);
                break;
            default:
                _logger.LogWarning("Invalid JobUpdatedEvent data: Field {FieldName} not found", notification.Event.Data.FieldName);
                return;
        }

        await _jobViewService.UpdateJobInfoViewAsync(jobInfo, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<JobDeletedEvent> notification, CancellationToken cancellationToken)
    {
        await _jobViewService.DeleteJobInfoViewAsync(notification.Event.AggregateId, cancellationToken);
    }
}
