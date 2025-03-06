using ATSourcing.Api.JobApplications.Contracts;
using ATSourcing.Application.JobApplications.Requests.Commands;
using MediatR;

namespace ATSourcing.Api.JobApplications;

public static class JobApplicationEndpoints
{
    public static WebApplication MapJobApplicationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/v1/job-applications");

        group.MapPost("{jobApplicationId:guid}/answer", AddAnswer);

        return app;
    }

    private static async Task<IResult> AddAnswer(Guid jobApplicationId, AnswerContract contract, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new AnswerJobApplicationCommand(jobApplicationId, contract.Answer),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok();
    }
}