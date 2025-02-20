using ATSourcing.Domain.Candidates.Events;
using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure.Interfaces;
using ESFrame.Insfrastructure.Models;
using System.Text.Json;

namespace ATSourcing.Infrastructure.Candidates;

public class DomainEventConverterModule : IDomainEventConverterModule<Guid>
{
    public IDomainEvent<Guid>? ConvertFromModel(DomainEventModel domainEvent)
    {
        return domainEvent.Name switch
        {
            nameof(CandidateCreatedEvent) => new CandidateCreatedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<CandidateCreatedEventData>(domainEvent.DataJson!)!, domainEvent.TimeStamp),
            nameof(CandidateDeletedEvent) => new CandidateDeletedEvent(Guid.Parse(domainEvent.AggregateId), domainEvent.TimeStamp),
            nameof(CandidateUpdatedEvent) => new CandidateUpdatedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<CandidateUpdatedEventData>(domainEvent.DataJson!)!, domainEvent.TimeStamp),
            _ => null
        };
    }

    public DomainEventModel? ConvertToModel(IDomainEvent<Guid> domainEvent)
    {
        return null;
    }
}
