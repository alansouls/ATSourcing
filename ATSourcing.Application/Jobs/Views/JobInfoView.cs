using ATSourcing.Domain.ValueObjects;

namespace ATSourcing.Application.Jobs.Views;

public class JobInfoView
{
    public Guid Id => JobId;
    public Guid JobId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<Guid> Candidates { get; set; } = [];

    public DateTimeOffset ApplicationDeadline { get; set; }

    public int VacancyCount { get; set; }

    public DecimalRange? SalaryRange { get; set; }
}
