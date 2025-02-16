using ATSourcing.Domain.ValueObjects;

namespace ATSourcing.Api.Jobs.Contracts;

public class CreateJobContract
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset ApplicationDeadline { get; set; }
    public int VacancyCount { get; set; }

    public DecimalRange? SalaryRange { get; set; }
}