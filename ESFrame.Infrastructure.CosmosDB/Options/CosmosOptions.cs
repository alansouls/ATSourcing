namespace ESFrame.Infrastructure.CosmosDB.Options;

public class CosmosOptions
{
    public string OptionsName => "CosmosDB";

    public required string DatabaseId { get; set; }

    public required string ConnectionString { get; set; }
}
