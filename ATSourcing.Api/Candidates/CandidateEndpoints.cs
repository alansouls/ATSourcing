using ATSourcing.Api.Candidates.Contracts;
using ATSourcing.Application.Candidates.Requests.Commands;
using MediatR;

namespace ATSourcing.Api.Candidates;

public static class CandidateEndpoints
{
    public static WebApplication MapCandidateEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/v1/candidates");

        group.MapPost("", CreateCandidate);

        return app;
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
}