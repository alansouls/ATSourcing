namespace ESFrame.Insfrastructure.Models;

public class DomainEventModel
{
    public required string AggregateId { get; set; }
    
    public required string Name { get; set; }
    
    public DateTimeOffset TimeStamp { get; set; }
    
    public string? DataJson { get; set; }
    
    public required string TypeName { get; set; }
}