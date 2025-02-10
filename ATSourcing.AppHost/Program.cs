using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddAzureCosmosDB("cosmos-db")
    .AddDatabase("atsourcing");

if (builder.Environment.IsDevelopment())
{
    cosmos
        .RunAsEmulator(emulator =>
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                emulator
                    .WithEnvironment("PROTOCOL", "https")
                    .WithImageTag("vnext-preview");
            }
            else
            {
                emulator
                    .WithEnvironment("PROTOCOL", "https");
            }

            emulator.WithGatewayPort(8081);

            emulator.WithDataVolume("data")
                .WithLifetime(ContainerLifetime.Persistent);
        });
}

var dbConfigurer = builder.AddProject<ATSourcing_CosmosDBConfigurer>("db-configurer")
    .WithReference(cosmos)
    .WaitFor(cosmos);

var api = builder.AddProject<ATSourcing_Api>("api")
    .WithReference(cosmos)
    .WaitFor(dbConfigurer);

builder.Build().Run();
