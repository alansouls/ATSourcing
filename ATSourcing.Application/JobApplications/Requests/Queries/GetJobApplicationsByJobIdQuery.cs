using ATSourcing.Application.JobApplications.Views;
using ESFrame.Application.Interfaces;
using ESFrame.Application.Views;
using MediatR;

namespace ATSourcing.Application.JobApplications.Requests.Queries;

public record GetJobApplicationsByJobIdQuery(Guid JobId, int Page, int PageSize, string? Sort, string? SortDirection)
    : IQuery<ViewPagingResult<JobApplicationItemView>>;