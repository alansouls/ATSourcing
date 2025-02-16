using ATSourcing.Domain.ValueObjects;
using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Jobs.Requests.Commands;

public record CreateJobCommand(string Title, string Description, DateTimeOffset ApplicationDeadline, int VacancyCount, DecimalRange? SalaryRange) : ICommand<Guid>;
