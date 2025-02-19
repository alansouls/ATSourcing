using ATSourcing.Domain.JobApplications.Events;
using ATSourcing.Domain.Jobs.Events;
using ATSourcing.Infrastructure.JobApplications.SerializableEventDatas;
using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure.Interfaces;
using ESFrame.Insfrastructure.Models;
using System.Text.Json;

namespace ATSourcing.Infrastructure.JobApplications;

internal class DomainEventConverterModule : IDomainEventConverterModule<Guid>
{
    public IDomainEvent<Guid>? ConvertFromModel(DomainEventModel domainEvent)
    {
        return domainEvent.Name switch
        {
            nameof(JobApplicationCreatedEvent) => new JobApplicationCreatedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<SerializableJobApplicationCreatedEventData>(domainEvent.DataJson!)!.ToJobApplicationCreatedEventData(), domainEvent.TimeStamp),
            nameof(JobApplicationAnswerAddedEvent) => new JobApplicationAnswerAddedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<JobApplicationAnswerAddedEventData>(domainEvent.DataJson!)!, domainEvent.TimeStamp),
            nameof(JobApplicationCurrentStepApprovedEvent) => new JobApplicationCurrentStepApprovedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<SerializableJobApplicationCurrentStepApprovedEventData>(domainEvent.DataJson!)!.ToJobApplicationCurrentStepApprovedEventData(), domainEvent.TimeStamp),
            nameof(JobApplicationCurrentStepRejectedEvent) => new JobApplicationCurrentStepRejectedEvent(Guid.Parse(domainEvent.AggregateId),
                JsonSerializer.Deserialize<SerializableJobApplicationCurrentStepRejectedEventData>(domainEvent.DataJson!)!.ToJobApplicationCurrentStepRejectedEventData(), domainEvent.TimeStamp),
            _ => null
        };
    }
    public DomainEventModel? ConvertToModel(IDomainEvent<Guid> domainEvent)
    {
        return domainEvent switch
        {
            JobApplicationCreatedEvent createdEvent => new DomainEventModel
            {
                Id = Guid.NewGuid(),
                AggregateId = domainEvent.AggregateId.ToString(),
                DataJson = JsonSerializer.Serialize(SerializableJobApplicationCreatedEventData.FromJobApplicationCreatedEventData(createdEvent.Data)),
                Name = domainEvent.Name,
                TimeStamp = domainEvent.TimeStamp
            },
            JobApplicationCurrentStepApprovedEvent currentStepApprovedEvent => new DomainEventModel
            {
                Id = Guid.NewGuid(),
                AggregateId = domainEvent.AggregateId.ToString(),
                DataJson = JsonSerializer.Serialize(SerializableJobApplicationCurrentStepApprovedEventData.FromJobApplicationCurrentStepApprovedEventData(currentStepApprovedEvent.Data)),
                Name = domainEvent.Name,
                TimeStamp = domainEvent.TimeStamp
            },
            JobApplicationCurrentStepRejectedEvent currentStepRejectedEvent => new DomainEventModel
            {
                Id = Guid.NewGuid(),
                AggregateId = domainEvent.AggregateId.ToString(),
                DataJson = JsonSerializer.Serialize(SerializableJobApplicationCurrentStepRejectedEventData.FromJobApplicationCurrentStepRejectedEventData(currentStepRejectedEvent.Data)),
                Name = domainEvent.Name,
                TimeStamp = domainEvent.TimeStamp
            },
            _ => null
        };
    }
}
