namespace ATSourcing.Application.Candidates.Views;

public class CandidateInfoView
{
    public Guid Id => CandidateId;
    public Guid CandidateId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public Guid UserId { get; set; }
}
