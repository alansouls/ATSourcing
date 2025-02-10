using ATSourcing.Application.Candidates.Interfaces;
using ATSourcing.Application.Candidates.Views;
using ESFrame.Application.Views;
using ESFrame.Infrastructure.CosmosDB.Interfaces;
using Microsoft.Azure.Cosmos;

namespace ATSourcing.Infrastructure.Candidates;

internal class CandidateViewService : ICandidateViewService
{
    private const string CandidateInfoViewContainerName = "CandidateInfoView";
    private const string CandidateInfoViewPartitionKeyPath = "/candidateId";
    private readonly IContainerFactory _containerFactory;

    public CandidateViewService(IContainerFactory containerFactory)
    {
        _containerFactory = containerFactory;
    }

    public async Task DeleteCandidateInfoViewAsync(Guid candidateId, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(CandidateInfoViewContainerName,
            CandidateInfoViewPartitionKeyPath, 
            cancellationToken);

        await container.DeleteItemAsync<CandidateInfoView>(candidateId.ToString(), new PartitionKey(candidateId.ToString()),
            cancellationToken: cancellationToken);
    }

    public async Task<CandidateInfoView?> GetCandidateInfoById(Guid candidateId, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(CandidateInfoViewContainerName,
            CandidateInfoViewPartitionKeyPath,
            cancellationToken);

        var query = container.GetItemQueryIterator<CandidateInfoView>(new QueryDefinition(
            $"SELECT * FROM c WHERE c.id = '{candidateId}'"), requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(candidateId.ToString())
            });

        var response = await query.ReadNextAsync(cancellationToken);

        return response.FirstOrDefault();
    }

    public async Task<ViewPagingResult<CandidateInfoView>> GetCandidatesInfo(ViewPagingParameters pagingParameters, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(CandidateInfoViewContainerName,
            CandidateInfoViewPartitionKeyPath,
            cancellationToken);

        var queryText = "SELECT * FROM c";

        queryText += $" ORDER BY c.{pagingParameters.Sort} {pagingParameters.SortDirection}";
        queryText += $" OFFSET {(pagingParameters.Page - 1) * pagingParameters.PageSize} LIMIT {pagingParameters.PageSize + 1}";

        var query = container.GetItemQueryIterator<CandidateInfoView>(new QueryDefinition(queryText));

        var result = new List<CandidateInfoView>();

        int actualPageSize = 0;

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(cancellationToken);

            actualPageSize += response.Count;
            result.AddRange(response);
        }

        return new ViewPagingResult<CandidateInfoView>
        {
            Data = result.Take(Math.Min(actualPageSize, pagingParameters.PageSize)).ToList(),
            Page = pagingParameters.Page,
            PageSize = Math.Min(actualPageSize, pagingParameters.PageSize),
            HasNextPage = actualPageSize > pagingParameters.PageSize
        };
    }

    public async Task InsertCandidateInfoViewAsync(CandidateInfoView candidateInfoView, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(CandidateInfoViewContainerName,
            CandidateInfoViewPartitionKeyPath,
            cancellationToken);

        await container.CreateItemAsync(candidateInfoView, new PartitionKey(candidateInfoView.CandidateId.ToString()),
            cancellationToken: cancellationToken);
    }

    public async Task UpdateCandidateInfoViewAsync(CandidateInfoView candidateInfoView, CancellationToken cancellationToken)
    {
        var container = await _containerFactory.GetOrCreateViewContainerAsync(CandidateInfoViewContainerName,
            CandidateInfoViewPartitionKeyPath,
            cancellationToken);

        await container.UpsertItemAsync(candidateInfoView, new PartitionKey(candidateInfoView.CandidateId.ToString()),
            cancellationToken: cancellationToken);
    }
}
