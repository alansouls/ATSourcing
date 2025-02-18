using ATSourcing.Domain.Jobs.Events;
using ATSourcing.Infrastructure.Jobs.SerializableEventDatas;
using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure.Interfaces;
using ESFrame.Insfrastructure.Models;
using System.Text.Json;

namespace ATSourcing.Infrastructure.Jobs;

internal class DomainEventConverterModule : IDomainEventConverterModule<Guid>
{
    public IDomainEvent<Guid>? ConvertFromModel(DomainEventModel domainEvent)
    {
        return domainEvent.Name switch
        {
            nameof(JobCreatedEvent) => new JobCreatedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<SerializableJobCreatedEventData>(domainEvent.DataJson!)!.ToJobCreatedEventData(), domainEvent.TimeStamp),
            nameof(JobDeletedEvent) => new JobDeletedEvent(Guid.Parse(domainEvent.AggregateId), domainEvent.TimeStamp),
            nameof(JobUpdatedEvent) => new JobUpdatedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<JobUpdatedEventData>(domainEvent.DataJson!)!, domainEvent.TimeStamp),
            nameof(JobCandidateApplicationAddedEvent) => new JobCandidateApplicationAddedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<JobCandidateApplicationAddedEventData>(domainEvent.DataJson!)!, domainEvent.TimeStamp),
            nameof(JobCandidateApplicationRemovedEvent) => new JobCandidateApplicationRemovedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<JobCandidateApplicationRemovedEventData>(domainEvent.DataJson!)!, domainEvent.TimeStamp),
            _ => null
        };
    }
}