using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

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
    public static Delegate HandleRequest => (IBus bus, OrderRequest request) =>
    {
        // TODO store orders (in-memory)
        bus.Publish(new OrderPlaced(request.id));
        // TODO configure outbox pattern
        return TypedResults.Ok(new { OrderId = request.id });
    };
}

public sealed record OrderPlaced(Guid id);

public sealed record OrderRequest(Guid id, OrderRequest.Item[] Items)
{
    public sealed record Item(string SKU, int Amount);
}
