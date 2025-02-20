using ATSourcing.Domain.StepDefinitions.Enums;

namespace ATSourcing.Application.JobApplications.Views;

public class JobApplicationItemView
{
    public Guid Id => JobApplicationId;
    public Guid JobApplicationId { get; set; }
    
    public Guid JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    
    public Guid CandidateId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    
    public StepState CurrentState { get; set; }
    public string CurrentStepTitle { get; set; } = string.Empty;
}