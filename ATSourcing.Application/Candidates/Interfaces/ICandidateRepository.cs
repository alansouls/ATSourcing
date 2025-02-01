using ATSourcing.Domain.Candidates;
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Candidates.Interfaces;

public interface ICandidateRepository : IRepository<Candidate, Guid>
{
    
}