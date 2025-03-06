using ATSourcing.Api.Jobs.Contracts;
using ATSourcing.Application.Jobs.Requests.Commands;
using ATSourcing.Application.Jobs.Requests.Queries;
using MediatR;

namespace ATSourcing.Api.Jobs;

public static class JobEndpoints
{
    public static WebApplication MapJobEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/v1/jobs");

        group.MapGet("", GetJobs);

        group.MapGet("{jobId:guid}", GetJob);

        group.MapPost("", CreateJob);

        group.MapPatch("{jobId:guid}", UpdateJob);

        group.MapDelete("{jobId:guid}", DeleteJob);

        group.MapPost("{jobId:guid}/application", ApplyForJob);

        return app;
    }

    private static async Task<IResult> GetJob(Guid jobId, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetJobQuery(jobId),
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

    private static async Task<IResult> GetJobs(int page, int? pageSize, string? sort, string? sortDirection,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetJobsQuery(page, pageSize ?? 10, sort, sortDirection),
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

    private static async Task<IResult> CreateJob(IMediator mediator, CreateJobContract contract,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateJobCommand(contract.Title,
                contract.Description,
                contract.ApplicationDeadline,
                contract.VacancyCount,
                contract.SalaryRange,
                contract.StepFlowDefinition
            ),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok(new { JobId = result.Value });
    }

    private static async Task<IResult> UpdateJob(Guid jobId,
        UpdateJobContract contract,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new UpdateJobCommand(jobId,
                contract.Title,
                contract.Description,
                contract.VacancyCount,
                contract.ApplicationDeadline,
                contract.SalaryRange
            ),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok();
    }

    private static async Task<IResult> DeleteJob(Guid jobId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteJobCommand(jobId),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok();
    }

    private static async Task<IResult> ApplyForJob(Guid jobId, AddApplicationContract contract, IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new AddCandidateApplicationCommand(jobId, contract.CandidateId),
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest();
        }

        return Results.Ok();
    }
}