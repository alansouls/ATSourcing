using ATSourcing.Domain.ValueObjects;
using ESFrame.Application.Interfaces;
using ESFrame.Application.Models;

namespace ATSourcing.Application.Jobs.Requests.Commands;

public record UpdateJobCommand(Guid JobId,
    string? Title, 
    string? Description,
    int? VacancyCount,
    DateTimeOffset? ApplicationDeadline,
    NullableFieldUpdateWrapper<DecimalRange?> SalaryRange) : ICommand;