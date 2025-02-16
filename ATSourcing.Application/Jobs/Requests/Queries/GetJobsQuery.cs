using ATSourcing.Application.Jobs.Views;
using ESFrame.Application.Interfaces;
using ESFrame.Application.Views;

namespace ATSourcing.Application.Jobs.Requests.Queries;

public record GetJobsQuery(int Page, int PageSize, string? Sort, string? SortDirection) : IQuery<ViewPagingResult<JobInfoView>>;
