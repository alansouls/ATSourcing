using FluentResults;
using MediatR;

namespace ESFrame.Application.Interfaces;

public interface IQuery<TResult> : IRequest<Result<TResult>>;
