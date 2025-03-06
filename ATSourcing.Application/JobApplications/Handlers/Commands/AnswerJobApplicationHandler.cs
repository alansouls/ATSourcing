using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Application.JobApplications.Requests.Commands;
using ATSourcing.Domain.StepDefinitions;
using ATSourcing.Domain.StepDefinitions.Enums;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.JobApplications.Handlers.Commands;

public class AnswerJobApplicationHandler : IRequestHandler<AnswerJobApplicationCommand, Result>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly TimeProvider _timeProvider;

    public AnswerJobApplicationHandler(IJobApplicationRepository jobApplicationRepository, TimeProvider timeProvider)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(AnswerJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var jobApplication = await _jobApplicationRepository.GetByIdAsync(request.JobApplicationId, cancellationToken);

        if (jobApplication is null)
        {
            return Result.Fail(new NotFoundError(nameof(jobApplication), request.JobApplicationId.ToString()));
        }

        Result result;

        if (jobApplication.CurrentStep is ConversationStep conversationStep &&
            conversationStep.State == StepState.PendingCandidate)
        {
            result = jobApplication.AddCandidateAnswer(request.Answer, _timeProvider.GetUtcNow());
        }
        else
        {
            result = jobApplication.AddRecruiterAnswer(request.Answer, _timeProvider.GetUtcNow());
        }

        if (result.IsFailed)
        {
            return result;
        }

        return await _jobApplicationRepository.SaveChangesAsync(jobApplication, cancellationToken);
    }
}