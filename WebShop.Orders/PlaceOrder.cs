using System.Collections.Immutable;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace WebShop.Orders;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder @this)
    {
        @this.MapPost("orders", PlaceOrder.HandleRequest);
        return @this;
    }
}

public static class PlaceOrder
{
    public static Delegate HandleRequest => (IBus bus, OderStore orders, OrderRequest request) =>
    {
        var order = request.ToOrder();

        // TODO configure outbox pattern
        orders.Save(order);
        bus.Publish(new OrderPlaced(order));

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

    public Order ToOrder(/* Catalog catalog */) =>
        new(Id, Items.Select(i => i.ToOrderItem( /* catalog */)).ToImmutableList());
}

public sealed record OrderPlaced(Guid Id) // Events should be light-weight; ideally only an ID
{
    internal OrderPlaced(Order order) : this(order.Id) { }
}
