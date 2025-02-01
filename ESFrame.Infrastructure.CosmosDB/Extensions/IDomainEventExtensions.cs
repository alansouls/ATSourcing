using System.Text.Json;
using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure.Models;

namespace ESFrame.Infrastructure.CosmosDB.Extensions;

public static class IDomainEventExtensions
{
    public static DomainEventModel ToModel<TKey>(this IDomainEvent<TKey> domainEvent) where TKey : IEquatable<TKey>
    {
        return new DomainEventModel
        {
            AggregateId = domainEvent.AggregateId.ToString()!,
            Name = domainEvent.Name,
            TimeStamp = domainEvent.TimeStamp,
            DataJson = domainEvent switch
            {
                IDomainEvent<TKey, >
            }
        };
    }
}