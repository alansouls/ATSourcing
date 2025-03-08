using ATSourcing.Application.JobApplications.Interfaces;
using ATSourcing.Application.JobApplications.Views;
using ESFrame.Application.Views;
using ESFrame.Infrastructure.CosmosDB.Interfaces;
using Microsoft.Azure.Cosmos;

namespace ATSourcing.Infrastructure.JobApplications;

internal class JobApplicationViewService : BaseViewService, IJobApplicationViewService
{
    private const string JobApplicationInfoViewContainerName = "JobApplicationInfoView";
    private const string JobApplicationPartitionKeyPath = "/jobApplicationId";
    private const string JobApplicationItemViewContainerName = "JobApplicationItemView";

    public JobApplicationViewService(IContainerFactory containerFactory) : base(containerFactory)
    {
    }

    public Task DeleteJobApplicationInfoViewAsync(Guid jobApplicationId, CancellationToken cancellationToken)
    {
        return DeleteViewAsync(jobApplicationId, jobApplicationId.ToString(), JobApplicationInfoViewContainerName,
            JobApplicationPartitionKeyPath, cancellationToken);
    }

    public Task DeleteJobApplicationItemViewAsync(Guid jobApplicationId, CancellationToken cancellationToken)
    {
        return DeleteViewAsync(jobApplicationId, jobApplicationId.ToString(), JobApplicationItemViewContainerName,
            JobApplicationPartitionKeyPath, cancellationToken);
    }

    public Task<JobApplicationInfoView?> GetJobApplicationInfoViewById(Guid jobApplicationId, CancellationToken cancellationToken)
    {
        return GetViewByIdAsync<JobApplicationInfoView>(jobApplicationId, jobApplicationId.ToString(), JobApplicationInfoViewContainerName,
            JobApplicationPartitionKeyPath, cancellationToken);
    }

    public Task<ViewPagingResult<JobApplicationItemView>> GetJobApplicationItems(Guid? candidateId, Guid? jobId, ViewPagingParameters pagingParameters, CancellationToken cancellationToken)
    {
        var whereConditions = string.Empty;

        if (candidateId.HasValue)
        {
            whereConditions += $"c.candidateId = '{candidateId}'";
        }

        if (jobId.HasValue)
        {
            if (!string.IsNullOrWhiteSpace(whereConditions))
            {
                whereConditions += " AND ";
            }
            whereConditions += $"c.jobId = '{jobId}'";
        }

        return GetViewsAsync<JobApplicationItemView>(whereConditions,
            JobApplicationItemViewContainerName, JobApplicationPartitionKeyPath, pagingParameters, cancellationToken);
    }

    public Task InsertJobApplicationInfoViewAsync(JobApplicationInfoView jobApplicationInfoView, CancellationToken cancellationToken)
    {
        return InsertViewAsync(jobApplicationInfoView, jobApplicationInfoView.JobApplicationId.ToString(), JobApplicationInfoViewContainerName,
            JobApplicationPartitionKeyPath, cancellationToken);
    }

    public Task InsertJobApplicationItemViewAsync(JobApplicationItemView jobApplicationItemView, CancellationToken cancellationToken)
    {
        return InsertViewAsync(jobApplicationItemView, jobApplicationItemView.JobApplicationId.ToString(), JobApplicationItemViewContainerName,
            JobApplicationPartitionKeyPath, cancellationToken);
    }

    public Task UpdateJobApplicationInfoViewAsync(JobApplicationInfoView jobApplicationInfoView, CancellationToken cancellationToken)
    {
        return UpdateViewAsync(jobApplicationInfoView, jobApplicationInfoView.JobApplicationId.ToString(), JobApplicationInfoViewContainerName,
            JobApplicationPartitionKeyPath, cancellationToken);
    }

    public Task UpdateJobApplicationItemViewAsync(JobApplicationItemView jobApplicationItemView, CancellationToken cancellationToken)
    {
        return UpdateViewAsync(jobApplicationItemView, jobApplicationItemView.JobApplicationId.ToString(), JobApplicationItemViewContainerName,
            JobApplicationPartitionKeyPath, cancellationToken);
    }
}
