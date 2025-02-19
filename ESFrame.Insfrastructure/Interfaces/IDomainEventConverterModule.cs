using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure.Models;

namespace ESFrame.Insfrastructure.Interfaces;

public interface IDomainEventConverterModule;

public interface IDomainEventConverterModule<TKey> : IDomainEventConverterModule where TKey : IEquatable<TKey>
{
    IDomainEvent<TKey>? ConvertFromModel(DomainEventModel domainEvent);
    DomainEventModel? ConvertToModel(IDomainEvent<TKey> domainEvent);
}