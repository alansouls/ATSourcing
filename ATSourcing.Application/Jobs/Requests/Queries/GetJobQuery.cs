using ATSourcing.Application.Jobs.Views;
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Jobs.Requests.Queries;

public record GetJobQuery(Guid JobId) : IQuery<JobInfoView?>;
