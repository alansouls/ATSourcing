using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.Jobs.Interfaces;
using ATSourcing.Application.Jobs.Requests.Commands;
using ESFrame.Application.Errors;
using FluentResults;
using MediatR;

namespace ATSourcing.Application.Jobs.Handlers.Commands;

internal class AddCandidateApplicationHandler : IRequestHandler<AddCandidateApplicationCommand, Result>
{
    private readonly TimeProvider _timeProvider;
    private readonly IJobRepository _jobRepository;
    private readonly ICandidateViewService _candidateViewService;

    public AddCandidateApplicationHandler(IJobRepository jobRepository, ICandidateViewService candidateViewService, 
        TimeProvider timeProvider)
    {
        _jobRepository = jobRepository;
        _candidateViewService = candidateViewService;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(AddCandidateApplicationCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateViewService.GetCandidateInfoById(request.CandidateId, cancellationToken);

        if (candidate is null)
        {
            return Result.Fail(new NotFoundError(nameof(candidate), request.CandidateId.ToString()));
        }
        
        var job = await _jobRepository.GetByIdAsync(request.JobId, cancellationToken);

        if (job is null)
        {
            return Result.Fail(new NotFoundError(nameof(job), request.JobId.ToString()));
        }
        
        job.AddCandidateApplication(candidate.Id, _timeProvider.GetUtcNow());
        
        return await _jobRepository.SaveChangesAsync(job, cancellationToken);
    }
}