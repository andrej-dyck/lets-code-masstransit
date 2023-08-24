using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace WebShop.Orders;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder @this)
    {
        @this.MapGet("orders", ListOrders.HandleRequest);
        @this.MapPost("orders", PlaceOrder.HandleRequest);
        return @this;
    }
}
