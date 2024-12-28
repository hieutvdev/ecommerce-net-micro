using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.Events.Domain;

public class OrderCreateEventHandler
(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEvent> logger)
: INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain event handled: {DomainEvent}", domainEvent.GetType().Name);
        if (await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var orderCreateIntegrationEvent = domainEvent.order.ToOrderDto();
            await publishEndpoint.Publish(orderCreateIntegrationEvent, cancellationToken);
        }
    }
}