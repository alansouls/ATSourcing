using ATSourcing.Application.Jobs.Views;
using ATSourcing.Application.Jobs.Interfaces;
using ESFrame.Application.Views;
using ESFrame.Infrastructure.CosmosDB.Interfaces;
using Microsoft.Azure.Cosmos;

namespace ATSourcing.Infrastructure.Jobs;

internal class JobViewService : IJobViewService
{
    private const string JobInfoViewContainerName = "JobInfoView";
    private const string JobInfoViewPartitionKeyPath = "/jobId";
    private readonly IContainerFactory _containerFactory;

    public JobViewService(IContainerFactory containerFactory)
    {
        _containerFactory = containerFactory;
    }

    public async Task DeleteJobInfoViewAsync(Guid jobId, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(JobInfoViewContainerName,
            JobInfoViewPartitionKeyPath,
            cancellationToken);

        await container.DeleteItemAsync<JobInfoView>(jobId.ToString(), new PartitionKey(jobId.ToString()),
            cancellationToken: cancellationToken);
    }

    public async Task<JobInfoView?> GetJobInfoById(Guid jobId, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(JobInfoViewContainerName,
            JobInfoViewPartitionKeyPath,
            cancellationToken);

        var query = container.GetItemQueryIterator<JobInfoView>(new QueryDefinition(
            $"SELECT * FROM c WHERE c.id = '{jobId}'"), requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(jobId.ToString())
            });

        var response = await query.ReadNextAsync(cancellationToken);

        return response.FirstOrDefault();
    }

    public async Task<ViewPagingResult<JobInfoView>> GetJobsInfo(ViewPagingParameters pagingParameters, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(JobInfoViewContainerName,
            JobInfoViewPartitionKeyPath,
            cancellationToken);

        var queryText = "SELECT * FROM c";

        queryText += $" ORDER BY c.{pagingParameters.Sort} {pagingParameters.SortDirection}";
        queryText += $" OFFSET {(pagingParameters.Page - 1) * pagingParameters.PageSize} LIMIT {pagingParameters.PageSize + 1}";

        var query = container.GetItemQueryIterator<JobInfoView>(new QueryDefinition(queryText));

        var result = new List<JobInfoView>();

        int actualPageSize = 0;

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(cancellationToken);

            actualPageSize += response.Count;
            result.AddRange(response);
        }

        return new ViewPagingResult<JobInfoView>
        {
            Data = result.Take(Math.Min(actualPageSize, pagingParameters.PageSize)).ToList(),
            Page = pagingParameters.Page,
            PageSize = Math.Min(actualPageSize, pagingParameters.PageSize),
            HasNextPage = actualPageSize > pagingParameters.PageSize
        };
    }

    public async Task InsertJobInfoViewAsync(JobInfoView JobInfoView, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(JobInfoViewContainerName,
            JobInfoViewPartitionKeyPath,
            cancellationToken);

        await container.CreateItemAsync(JobInfoView, new PartitionKey(JobInfoView.JobId.ToString()),
            cancellationToken: cancellationToken);
    }

    public async Task UpdateJobInfoViewAsync(JobInfoView JobInfoView, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(JobInfoViewContainerName,
            JobInfoViewPartitionKeyPath,
            cancellationToken);

        await container.UpsertItemAsync(JobInfoView, new PartitionKey(JobInfoView.JobId.ToString()),
            cancellationToken: cancellationToken);
    }
}
