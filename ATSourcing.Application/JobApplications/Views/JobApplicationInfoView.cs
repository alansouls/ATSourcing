using ATSourcing.Domain.StepDefinitions.Enums;
using ATSourcing.Domain.StepDefinitions.Snapshots;

namespace ATSourcing.Application.JobApplications.Views;

public class JobApplicationInfoView
{
    public Guid Id => JobApplicationId;
    public Guid JobApplicationId { get; set; }
    
    public Guid JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string JobDescription { get; set; } = string.Empty;

    public Guid CandidateId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    
    public StepState CurrentState { get; set; }
    public required StepSnapshot CurrentStep { get; set; }
    public string NextStepTitle { get; set; } = string.Empty;
}