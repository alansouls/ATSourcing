using ATSourcing.Api.Candidates;
using ATSourcing.Application.Candidates.Requests.Commands;
using ATSourcing.Application.Extensions;
using ATSourcing.Infrastructure.Extensions;
using ESFrame.Infrastructure.CosmosDB.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddServiceDefaults();

builder.AddCosmosInfrastructure("cosmos-db");

builder.Services.AddApplication();

builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapCandidateEndpoints();

app.Run();