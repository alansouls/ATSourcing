namespace ESFrame.Insfrastructure.Models;

public class DomainEventModel
{
    public required Guid Id { get; set; }
    public required string AggregateId { get; set; }

    public required string Name { get; set; }

    public DateTimeOffset TimeStamp { get; set; }

    public string? DataJson { get; set; }
}