using ATSourcing.Domain.ValueObjects;
using ESFrame.Application.Models;

namespace ATSourcing.Api.Jobs.Contracts;

public class UpdateJobContract
{
    public string? Title { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public int? VacancyCount { get; set; }
    public DateTimeOffset? ApplicationDeadline { get; set; } = new();
    public NullableFieldUpdateWrapper<DecimalRange?> SalaryRange { get; set; } = new();
}
