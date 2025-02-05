using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddAzureCosmosDB("cosmos-db");

if (builder.Environment.IsDevelopment())
{
    cosmos
        .RunAsEmulator(emulator => emulator
            .WithImageTag("vnext-preview")
            .WithHttpsEndpoint(1234, 1234)
            .WithEnvironment("PROTOCOL", "https"));
}

var dbConfigurer = builder.AddProject<ATSourcing_CosmosDBConfigurer>("db-configurer")
    .WithReference(cosmos)
    .WaitFor(cosmos);

var api = builder.AddProject<ATSourcing_Api>("api")
    .WithReference(cosmos)
    .WaitFor(dbConfigurer);

builder.Build().Run();
