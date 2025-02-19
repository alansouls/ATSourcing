using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure.Models;

namespace ESFrame.Insfrastructure.Interfaces;

public interface IDomainEventModelConverter
{
    IDomainEvent<TKey> ToDomainEvent<TKey>(DomainEventModel domainEventModel) where TKey : IEquatable<TKey>;

    DomainEventModel ToDomainEventModel<TKey>(IDomainEvent<TKey> domainEvent) where TKey : IEquatable<TKey>;
}
