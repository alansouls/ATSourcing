using ESFrame.Application;
using ESFrame.Application.Interfaces;
using ESFrame.Domain.Interfaces;
using MediatR;

namespace ESFrame.Insfrastructure;

public class LocalDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _publisher;

    public LocalDomainEventDispatcher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task DispatchAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken) where TDomainEvent : IDomainEvent
    {
        var notificationType = typeof(ApplicationNotification<>).MakeGenericType(domainEvent.GetType());

        var notification = Activator.CreateInstance(notificationType,
            args: [domainEvent])!;

        return _publisher.Publish(notification, cancellationToken);
    }
}
