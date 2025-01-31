using FluentResults;
using MediatR;

namespace ESFrame.Application.Interfaces;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResult> : IRequest<Result<TResult>>;
