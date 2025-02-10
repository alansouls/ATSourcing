using ATSourcing.Application.Candidates.Views;
using ESFrame.Application.Interfaces;
using ESFrame.Application.Views;

namespace ATSourcing.Application.Candidates.Requests.Queries;

public record GetCandidatesQuery(int Page, int PageSize, string? Sort, string? SortDirection) : IQuery<ViewPagingResult<CandidateInfoView>>;
