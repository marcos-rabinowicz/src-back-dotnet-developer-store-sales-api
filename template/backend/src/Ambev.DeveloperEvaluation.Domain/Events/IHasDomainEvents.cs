using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
