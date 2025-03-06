using ATSourcing.Application.JobApplications.Views;
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.JobApplications.Requests.Queries;

public record GetJobApplicationsByCandidateIdQuery(
    Guid CandidateId,
    int Page,
    int PageSize,
    string? Sort,
    string? SortDirection) : IQuery<IEnumerable<JobApplicationInfoView>>;