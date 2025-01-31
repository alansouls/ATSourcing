using ESFrame.Domain.Interfaces;
using MediatR;

namespace ESFrame.Application;

public record ApplicationNotification<TEvent>(TEvent Event) : INotification where TEvent : IDomainEvent;