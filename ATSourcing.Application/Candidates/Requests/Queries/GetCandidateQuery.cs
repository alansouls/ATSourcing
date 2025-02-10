using ATSourcing.Application.Candidates.Views;
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Candidates.Requests.Queries;

public record GetCandidateQuery(Guid CandidateId) : IQuery<CandidateInfoView?>;
