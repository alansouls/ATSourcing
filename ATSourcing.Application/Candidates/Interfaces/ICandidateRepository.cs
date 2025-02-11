using ATSourcing.Domain.Candidates;
using ATSourcing.Domain.Candidates.Snapshots;
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Candidates.Interfaces;

public interface ICandidateRepository : IRepository<Candidate, CandidateSnapshot, Guid>
{
    
}