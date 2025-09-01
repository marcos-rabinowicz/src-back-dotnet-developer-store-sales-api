using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.DomainEvents.Handlers;

public sealed class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(SaleCreatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Sale created: {SaleId}", notification.SaleId);

        return Task.CompletedTask;
    }
}
