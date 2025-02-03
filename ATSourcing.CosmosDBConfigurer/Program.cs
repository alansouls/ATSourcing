using ATSourcing.CosmosDBConfigurer;
using ESFrame.Infrastructure.CosmosDB.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
{
    { "CosmosDB:DatabaseId", "atsourcing" }
}!);

builder.AddCosmosInfrastructure("cosmos-db");

builder.Services.AddHostedService<CosmosConfigurerService>();

IHost host = builder.Build();

host.Run();