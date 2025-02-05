namespace ATSourcing.Api.Candidates.Contracts;

public class CreateCandidateContract
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    
    public Guid UserId { get; set; }
}