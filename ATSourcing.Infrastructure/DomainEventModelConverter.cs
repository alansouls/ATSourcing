using ESFrame.Domain.Interfaces;
using ESFrame.Insfrastructure.Interfaces;
using ESFrame.Insfrastructure.Models;

namespace ATSourcing.Infrastructure;

internal class DomainEventModelConverter : IDomainEventModelConverter
{
    private readonly IEnumerable<IDomainEventConverterModule> _converters;

    public DomainEventModelConverter(IEnumerable<IDomainEventConverterModule> converters)
    {
        _converters = converters.ToList();
    }

    public IDomainEvent<TKey> ToDomainEvent<TKey>(DomainEventModel domainEventModel) where TKey : IEquatable<TKey>
    {
        foreach (var convert in _converters.OfType<IDomainEventConverterModule<TKey>>())
        {
            var domainEvent = convert.ConvertFromModel(domainEventModel);

            if (domainEvent != null)
            {
                return domainEvent;
            }
        }

        throw new NotSupportedException($"No converter found for domain event {domainEventModel.Name}");
    }
}
