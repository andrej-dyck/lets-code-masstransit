using System.Collections.Immutable;
using DotNet.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace WebShop.Orders;

public static class PlaceOrder
{
    public static Delegate HandleRequest => async (
        IPublishEndpoint events,
        OrderStore orders,
        UtcNow utcNow,
        OrderRequest request) =>
    {
        var order = request.ToOrder(utcNow());

        // TODO configure outbox pattern here
        await orders.Save(order);
        await events.Publish(new OrderPlaced(order));

        return TypedResults.Ok(new { OrderId = order.Id });
    };
}

public sealed record OrderRequest(Guid Id, OrderRequest.Item[] Items)
{
    public sealed record Item(string SKU, int Amount)
    {
        public Order.Item ToOrderItem() =>
            // TODO use sales catalog to determine name and price 
            new(Sku: SKU, Name: string.Empty, Amount: Amount, Price: new Random().Next(0, 100));
    }

    public Order ToOrder(DateTimeOffset time /*, SalesCatalog catalog */) =>
        new(Id, time, Items.Select(i => i.ToOrderItem( /* catalog */)).ToImmutableList());
}

/**
 * Event - It's a (domain) contract
 *
 * This will be serialized / deserialized as a message.
 *
 * Further, messages must be light-weight; ideally only an ID.
 */
public sealed record OrderPlaced(Guid Id)
{
    internal OrderPlaced(Order order) : this(order.Id) { }
}
