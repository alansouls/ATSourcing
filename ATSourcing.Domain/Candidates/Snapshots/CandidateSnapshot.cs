namespace ATSourcing.Domain.Candidates.Snapshots;

public class CandidateSnapshot
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required int Age { get; set; }
    
    public required string Email { get; set; }
    
    public required Guid UserId { get; set; }
}