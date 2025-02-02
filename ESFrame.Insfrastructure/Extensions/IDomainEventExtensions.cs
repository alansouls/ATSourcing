using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure.Models;
using System.Text.Json;

namespace ESFrame.Insfrastructure.Extensions;

public static class IDomainEventExtensions
{
    public static DomainEventModel ToModel<TKey>(this IDomainEvent<TKey> domainEvent) where TKey : IEquatable<TKey>
    {
        return new DomainEventModel
        {
            Id = Guid.NewGuid(),
            AggregateId = domainEvent.AggregateId.ToString()!,
            Name = domainEvent.Name,
            TimeStamp = domainEvent.TimeStamp,
            DataJson = domainEvent switch
            {
                IDomainEventWithData<TKey> eventWithData => JsonSerializer.Serialize(eventWithData.GenericData),
                _ => null
            }
        };
    }
}