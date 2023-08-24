using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace WebShop.Orders;

public static class OrdersEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder @this)
    {
        @this.MapPost(
            "orders",
            (OrderRequest request) => TypedResults.Ok(new { OrderId = request.id })
        );
        return @this;
    }
}

public sealed record OrderRequest(Guid id, OrderRequest.Item[] Items)
{
    public sealed record Item(string SKU, int Amount);
}
