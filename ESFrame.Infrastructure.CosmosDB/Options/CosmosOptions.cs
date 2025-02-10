namespace ESFrame.Infrastructure.CosmosDB.Options;

public class CosmosOptions
{
    public const string OptionsName = "CosmosDB";

    public string DomainDatabaseId { get; set; } = string.Empty;

    public string ViewDatabaseId { get; set; } = string.Empty;
}
