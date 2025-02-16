using ATSourcing.Application.Jobs.Views;
using ESFrame.Application.Views;

namespace ATSourcing.Application.Jobs.Interfaces;

public interface IJobViewService
{
    Task<JobInfoView?> GetJobInfoById(Guid jobId, CancellationToken cancellationToken);
    Task<ViewPagingResult<JobInfoView>> GetJobsInfo(ViewPagingParameters pagingParameters, CancellationToken cancellationToken);

    Task InsertJobInfoViewAsync(JobInfoView jobInfoView, CancellationToken cancellationToken);
    Task UpdateJobInfoViewAsync(JobInfoView jobInfoView, CancellationToken cancellationToken);
    Task DeleteJobInfoViewAsync(Guid jobId, CancellationToken cancellationToken);
}
