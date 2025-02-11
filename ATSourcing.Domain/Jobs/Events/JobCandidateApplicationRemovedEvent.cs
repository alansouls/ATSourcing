using ESFrame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSourcing.Domain.Jobs.Events;

public record JobCandidateApplicationRemovedEventData(Guid CandidateId);

public class JobCandidateApplicationRemovedEvent : DomainEvent<Guid, JobCandidateApplicationRemovedEventData>
{
    public JobCandidateApplicationRemovedEvent(Guid jobId, JobCandidateApplicationRemovedEventData data, DateTimeOffset timeStamp)
    {
        AggregateId = jobId;
        Data = data;
        TimeStamp = timeStamp;
    }

    public override string Name => nameof(JobCandidateApplicationAddedEvent);
}
