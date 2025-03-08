using ATSourcing.Application.JobApplications.Views;
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.JobApplications.Requests.Queries;

public record GetJobApplicationByIdQuery(Guid JobApplicationId) : IQuery<JobApplicationInfoView>;
