using ATSourcing.Domain.Jobs;
using ATSourcing.Domain.Jobs.Snapshots;
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Jobs.Interfaces;

public interface IJobRepository : IRepository<Job, JobSnapshot, Guid>
{
}
