using ATSourcing.Api.JobApplications.Contracts;
using ATSourcing.Application.JobApplications.Requests.Commands;
using ATSourcing.Application.JobApplications.Requests.Queries;
using MediatR;

namespace ATSourcing.Api.JobApplications;

public static class JobApplicationEndpoints
{
    public static WebApplication MapJobApplicationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/v1/job-applications");

        group.MapPost("{jobApplicationId:guid}/answer", AddAnswer);

        group.MapGet("candidates/{candidateId:guid}", GetJobApplicationsByCandidateId);

        group.MapGet("jobs/{jobId:guid}", GetJobApplicationsByJobId);

        group.MapGet("{jobApplicationId:guid}", GetJobApplicationById);

        group.MapPost("{jobApplicationId:guid}/approve-step", ApproveStep);

        group.MapPost("{jobApplicationId:guid}/reject-step", RejectStep);

        return app;
    }

    private static async Task<IResult> GetJobApplicationsByCandidateId(Guid candidateId, int page, int? pageSize, string? sort, string? sortDirection, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetJobApplicationsByCandidateIdQuery(candidateId, page, pageSize ?? 10, sort, sortDirection);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetJobApplicationsByJobId(Guid jobId, int page, int? pageSize, string? sort, string? sortDirection, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetJobApplicationsByJobIdQuery(jobId, page, pageSize ?? 10, sort, sortDirection);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetJobApplicationById(Guid jobApplicationId, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetJobApplicationByIdQuery(jobApplicationId);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok(result.Value);
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

    private static async Task<IResult> ApproveStep(Guid jobApplicationId, ApproveContract contract, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ApproveJobApplicationStepCommand(jobApplicationId, contract.FinalObservations),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok();
    }

    private static async Task<IResult> RejectStep(Guid jobApplicationId, RejectContract contract, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RejectJobApplicationStepCommand(jobApplicationId, contract.FinalObservations),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok();
    }
}