using MassTransit;
using Microsoft.Extensions.Logging;
using WebShop.Orders;

namespace WebShop.Notifications;

// ReSharper disable once UnusedType.Global - Instantiated by MassTransit
public sealed class HandleOrderReceived : IConsumer<OrderPlaced>
{
    private readonly ILogger<HandleOrderReceived> _logger;
    public HandleOrderReceived(ILogger<HandleOrderReceived> logger) => _logger = logger;

    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        _logger.LogInformation("Event: {}", context.Message);
        // TODO notify customers
        return Task.CompletedTask;
    }
}
