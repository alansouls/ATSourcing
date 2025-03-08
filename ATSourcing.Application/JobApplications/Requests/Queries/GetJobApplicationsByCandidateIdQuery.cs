using ATSourcing.Application.JobApplications.Views;
using ESFrame.Application.Interfaces;
using ESFrame.Application.Views;

namespace ATSourcing.Application.JobApplications.Requests.Queries;

public record GetJobApplicationsByCandidateIdQuery(
    Guid CandidateId,
    int Page,
    int PageSize,
    string? Sort,
    string? SortDirection) : IQuery<ViewPagingResult<JobApplicationItemView>>;