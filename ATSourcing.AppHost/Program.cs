using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder
    .AddAzureCosmosDB("cosmos-db")
    .RunAsEmulator();

var api = builder.AddProject<ATSourcing_Api>("api")
    .WithReference(cosmos);

builder.Build().Run();
