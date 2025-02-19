using ATSourcing.Domain.JobApplications;
using ATSourcing.Domain.JobApplications.Snapshots;
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.JobApplications.Interfaces;

public interface IJobApplicationRepository : IRepository<JobApplication, JobApplicationSnapshot, Guid>
{
}
