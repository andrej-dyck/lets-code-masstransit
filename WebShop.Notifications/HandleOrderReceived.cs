using MassTransit;
using Microsoft.Extensions.Logging;
using WebShop.Orders;

namespace WebShop.Notifications;

// ReSharper disable once UnusedType.Global - Instantiated by MassTransit
public sealed class HandleNewOrders : IConsumer<OrderPlaced>
{
    private readonly ILogger<HandleNewOrders> _logger;
    public HandleNewOrders(ILogger<HandleNewOrders> logger) => _logger = logger;

    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        _logger.LogInformation("Event: {}", context.Message);
        // TODO notify customers
        return Task.CompletedTask;
    }
}
