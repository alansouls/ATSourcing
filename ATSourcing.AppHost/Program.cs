using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder
    .AddConnectionString("cosmos-db");

var api = builder.AddProject<ATSourcing_Api>("api")
    .WithReference(cosmos);

builder.Build().Run();
