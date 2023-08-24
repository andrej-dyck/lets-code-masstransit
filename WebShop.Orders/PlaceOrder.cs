using System.Collections.Immutable;
using DotNet.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace WebShop.Orders;

public static class PlaceOrder
{
    public static Delegate HandleRequest => async (IBus bus, OderStore orders, UtcNow utcNow, OrderRequest request) =>
    {
        var order = request.ToOrder(utcNow());

        // TODO configure outbox pattern
        await orders.Save(order);
        await bus.Publish(new OrderPlaced(order));

        return TypedResults.Ok(new { OrderId = request.Id });
    };
}

public sealed record OrderRequest(Guid Id, OrderRequest.Item[] Items)
{
    public sealed record Item(string SKU, int Amount)
    {
        // TODO use catalog to determine name price 
        public Order.Item ToOrderItem() =>
            new(Sku: SKU, Name: string.Empty, Amount: Amount, Price: 0m);
    }

    public Order ToOrder(DateTimeOffset time /*, Catalog catalog */) =>
        new(Id, time, Items.Select(i => i.ToOrderItem( /* catalog */)).ToImmutableList());
}

public sealed record OrderPlaced(Guid Id) // Events should be light-weight; ideally only an ID
{
    internal OrderPlaced(Order order) : this(order.Id) { }
}
