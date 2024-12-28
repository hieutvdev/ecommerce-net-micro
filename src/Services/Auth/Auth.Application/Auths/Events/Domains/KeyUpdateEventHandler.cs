using Auth.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Auths.Events.Domains;

public class KeyUpdateEventHandler
(ILogger<KeyUpdateEventHandler> logger)
: INotificationHandler<KeyUpdateEvent>
{
    public Task Handle(KeyUpdateEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}