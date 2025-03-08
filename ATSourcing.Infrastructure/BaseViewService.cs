using ATSourcing.Application.JobApplications.Views;
using ATSourcing.Application.Jobs.Views;
using ESFrame.Application.Views;
using ESFrame.Infrastructure.CosmosDB.Interfaces;
using Microsoft.Azure.Cosmos;

namespace ATSourcing.Infrastructure;

internal abstract class BaseViewService
{
    private readonly IContainerFactory _containerFactory;

    public BaseViewService(IContainerFactory containerFactory)
    {
        _containerFactory = containerFactory;
    }

    protected async Task InsertViewAsync<TView>(TView view, string partitionKey, string containerName, string partitionKeyPath, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(containerName,
            partitionKeyPath,
            cancellationToken);

        await container.CreateItemAsync(view, new PartitionKey(partitionKey),
            cancellationToken: cancellationToken);
    }

    protected async Task DeleteViewAsync(Guid viewId, string partitionKey, string containerName, string partitionKeyPath, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(containerName,
            partitionKeyPath,
            cancellationToken);

        await container.DeleteItemAsync<JobApplicationInfoView>(viewId.ToString(), new PartitionKey(partitionKey),
            cancellationToken: cancellationToken);
    }

    protected async Task UpdateViewAsync<TView>(TView view, string partitionKey, string containerName, string partitionKeyPath, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(containerName,
            partitionKeyPath,
            cancellationToken);

        await container.UpsertItemAsync(view, new PartitionKey(partitionKey),
            cancellationToken: cancellationToken);
    }

    protected async Task<ViewPagingResult<TView>> GetViewsAsync<TView>(string whereConditions,
        string containerName, 
        string partitionKeyPath,
        ViewPagingParameters pagingParameters,
        CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(containerName,
            partitionKeyPath,
            cancellationToken);

        var queryText = "SELECT * FROM c";

        if (!string.IsNullOrWhiteSpace(whereConditions))
        {
            queryText += $" WHERE {whereConditions}";
        }

        queryText += $" ORDER BY c.{pagingParameters.Sort} {pagingParameters.SortDirection}";
        queryText += $" OFFSET {(pagingParameters.Page - 1) * pagingParameters.PageSize} LIMIT {pagingParameters.PageSize + 1}";

        var query = container.GetItemQueryIterator<TView>(new QueryDefinition(queryText));

        var result = new List<TView>();

        int actualPageSize = 0;

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(cancellationToken);

            actualPageSize += response.Count;
            result.AddRange(response);
        }

        return new ViewPagingResult<TView>
        {
            Data = result.Take(Math.Min(actualPageSize, pagingParameters.PageSize)).ToList(),
            Page = pagingParameters.Page,
            PageSize = Math.Min(actualPageSize, pagingParameters.PageSize),
            HasNextPage = actualPageSize > pagingParameters.PageSize
        };
    }

    protected async Task<TView?> GetViewByIdAsync<TView>(Guid viewId, string partitionKey, string containerName, string partitionKeyPath, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(containerName,
            partitionKeyPath,
            cancellationToken);

        var query = container.GetItemQueryIterator<TView>(new QueryDefinition(
            $"SELECT * FROM c WHERE c.id = '{viewId}'"), requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey.ToString())
            });

        var response = await query.ReadNextAsync(cancellationToken);

        return response.FirstOrDefault();
    }
}
