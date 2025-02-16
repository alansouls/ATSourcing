using ESFrame.Application.Interfaces;

namespace ATSourcing.Application.Jobs.Requests.Commands;

public record DeleteJobCommand(Guid JobId) : ICommand;
