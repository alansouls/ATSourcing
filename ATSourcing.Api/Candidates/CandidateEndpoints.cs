using ATSourcing.Api.Candidates.Contracts;
using ATSourcing.Application.Candidates.Requests.Commands;
using ATSourcing.Application.Candidates.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;

namespace ATSourcing.Api.Candidates;

public static class CandidateEndpoints
{
    public static WebApplication MapCandidateEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/v1/candidates");

        group.MapGet("", GetCandidates);

        group.MapGet("{candidateId:guid}", GetCandidate);

        group.MapPost("", CreateCandidate);

        group.MapPatch("{candidateId:guid}", UpdateCandidate);

        group.MapDelete("{candidateId:guid}", DeleteCandidate);

        return app;
    }

    private static async Task<IResult> GetCandidate(Guid candidateId, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCandidateQuery(candidateId),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }
        else if (result.Value is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetCandidates(int page, int? pageSize, string? sort, string? sortDirection,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCandidatesQuery(page, pageSize ?? 10, sort, sortDirection),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }
        else if (result.Value is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> CreateCandidate(IMediator mediator, CreateCandidateContract contract, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateCandidateCommand(contract.FirstName,
                contract.LastName,
                contract.Age,
                contract.Email,
                contract.UserId
            ),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok(new { CandidateId = result.Value });
    }

    private static async Task<IResult> UpdateCandidate(Guid candidateId,
        UpdateCandidateContract contract,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new UpdateCandidateCommand(candidateId,
                contract.FirstName,
                contract.LastName,
                contract.Age,
                contract.Email
            ),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok();
    }

    private static async Task<IResult> DeleteCandidate(Guid candidateId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteCandidateCommand(candidateId),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok();
    }
}