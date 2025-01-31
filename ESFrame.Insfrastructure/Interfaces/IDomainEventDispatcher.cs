using ESFrame.Domain.Interfaces;

namespace ESFrame.Insfrastructure.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken) where TDomainEvent : IDomainEvent;
}
