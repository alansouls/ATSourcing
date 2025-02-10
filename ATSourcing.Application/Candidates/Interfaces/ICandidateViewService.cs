using ATSourcing.Application.Candidates.Views;
using ESFrame.Application.Views;

namespace ATSourcing.Application.Candidates.Interfaces;

public interface ICandidateViewService
{
    Task<CandidateInfoView?> GetCandidateInfoById(Guid candidateId, CancellationToken cancellationToken);
    Task<ViewPagingResult<CandidateInfoView>> GetCandidatesInfo(ViewPagingParameters pagingParameters, CancellationToken cancellationToken);

    Task InsertCandidateInfoViewAsync(CandidateInfoView candidateInfoView, CancellationToken cancellationToken);
    Task UpdateCandidateInfoViewAsync(CandidateInfoView candidateInfoView, CancellationToken cancellationToken);
    Task DeleteCandidateInfoViewAsync(Guid candidateId, CancellationToken cancellationToken);
}
