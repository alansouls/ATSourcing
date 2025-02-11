using ESFrame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSourcing.Domain.Jobs.Events;

public record JobCandidateApplicationAddedEventData(Guid CandidateId);

public class JobCandidateApplicationAddedEvent : DomainEvent<Guid, JobCandidateApplicationAddedEventData>
{
    public JobCandidateApplicationAddedEvent(Guid jobId, JobCandidateApplicationAddedEventData data, DateTimeOffset timeStamp)
    {
        AggregateId = jobId;
        Data = data;
        TimeStamp = timeStamp;
    }

    public override string Name => nameof(JobCandidateApplicationAddedEvent);
}
