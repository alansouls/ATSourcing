using ATSourcing.Application.JobApplications.Views;
using ESFrame.Application.Interfaces;
using ESFrame.Application.Views;

namespace ATSourcing.Application.JobApplications.Interfaces;

public interface IJobApplicationViewService : IViewService
{
    Task<JobApplicationInfoView?> GetJobApplicationInfoViewById(Guid jobApplicationId,
        CancellationToken cancellationToken);

    Task InsertJobApplicationInfoViewAsync(JobApplicationInfoView jobApplicationInfoView,
        CancellationToken cancellationToken);

    Task UpdateJobApplicationInfoViewAsync(JobApplicationInfoView jobApplicationInfoView,
        CancellationToken cancellationToken);

    Task DeleteJobApplicationInfoViewAsync(Guid jobApplicationId, CancellationToken cancellationToken);
    
    Task<ViewPagingResult<JobApplicationItemView>> GetJobApplicationsInfo(ViewPagingParameters pagingParameters,
        CancellationToken cancellationToken);

    Task InsertJobApplicationItemViewAsync(JobApplicationItemView jobApplicationInfoView,
        CancellationToken cancellationToken);

    Task UpdateJobApplicationItemViewAsync(JobApplicationItemView jobApplicationInfoView,
        CancellationToken cancellationToken);

    Task DeleteJobApplicationItemViewAsync(Guid jobApplicationId, CancellationToken cancellationToken);
}