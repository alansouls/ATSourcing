using MediatR;

namespace ATSourcing.Api.Candidates;

public static class CandidateEndpoints
{
    public static WebApplication AddCandidateEndpoins(this WebApplication app)
    {
        var group = app.MapGroup("api/v1/candidates");

        group.MapGet("", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetCandidatesQuery(), cancellationToken);
            if (result.IsFailed)
            {
                return Results.BadRequest();
            }
            return Results.Ok(result.Value);
        });
    }
}
