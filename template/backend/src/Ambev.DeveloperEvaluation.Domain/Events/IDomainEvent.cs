using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Marcador para eventos de domínio. Herda de MediatR.INotification
/// para permitir o publish/handle via MediatR sem acoplar nada extra.
/// </summary>
public interface IDomainEvent : INotification
{
}
