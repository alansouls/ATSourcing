using ESFrame.Domain.Interfaces;

namespace ATSourcing.Domain.Candidates.Snapshots;

public class CandidateSnapshot : IEntitySnapshot<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AggregateId { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required int Age { get; set; }
    
    public required string Email { get; set; }
    
    public required Guid UserId { get; set; }
    
    public bool IsDeleted { get; set; }
}