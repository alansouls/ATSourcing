using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.Candidates.Views;
using ATSourcing.Domain.Candidates;
using ATSourcing.Domain.Candidates.Events;
using ESFrame.Application;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ATSourcing.Application.Candidates.Handlers.Events;

internal class CandidateInfoViewUpdaterHandler : INotificationHandler<ApplicationNotification<CandidateCreatedEvent>>,
    INotificationHandler<ApplicationNotification<CandidateUpdatedEvent>>,
    INotificationHandler<ApplicationNotification<CandidateDeletedEvent>>
{
    private readonly ICandidateViewService _candidateViewService;
    private readonly ILogger<CandidateInfoViewUpdaterHandler> _logger;

    public CandidateInfoViewUpdaterHandler(ICandidateViewService candidateViewService, ILogger<CandidateInfoViewUpdaterHandler> logger)
    {
        _candidateViewService = candidateViewService;
        _logger = logger;
    }

    public async Task Handle(ApplicationNotification<CandidateCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var candidateInfoView = new CandidateInfoView
        {
            CandidateId = notification.Event.AggregateId,
            FirstName = notification.Event.Data.FirstName,
            LastName = notification.Event.Data.LastName,
            Email = notification.Event.Data.Email,
            Age = notification.Event.Data.Age,
            UserId = notification.Event.Data.UserId
        };

        await _candidateViewService.InsertCandidateInfoViewAsync(candidateInfoView, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<CandidateUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        var candidateInfo = await _candidateViewService.GetCandidateInfoById(notification.Event.AggregateId, cancellationToken);

        if (candidateInfo is null)
        {
            _logger.LogWarning("Candidate with id {CandidateId} not found", notification.Event.AggregateId);
            return;
        }

        switch (notification.Event.Data.FieldName)
        {
            case nameof(Candidate.FirstName):
                candidateInfo.FirstName = notification.Event.Data.NewValue;
                break;
            case nameof(Candidate.LastName):
                candidateInfo.LastName = notification.Event.Data.NewValue;
                break;
            case nameof(Candidate.Email):
                candidateInfo.Email = notification.Event.Data.NewValue;
                break;
            case nameof(Candidate.Age):
                candidateInfo.Age = int.Parse(notification.Event.Data.NewValue);
                break;
            default:
                _logger.LogWarning("Invalid CandidateUpdatedEvent data: Field {FieldName} not found", notification.Event.Data.FieldName);
                return;
        }

        await _candidateViewService.UpdateCandidateInfoViewAsync(candidateInfo, cancellationToken);
    }

    public async Task Handle(ApplicationNotification<CandidateDeletedEvent> notification, CancellationToken cancellationToken)
    {
        await _candidateViewService.DeleteCandidateInfoViewAsync(notification.Event.AggregateId, cancellationToken);
    }
}
