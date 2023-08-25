using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Orders;

public static class ListOrders
{
    public static Delegate HandleRequest => async (OrderStore orders, [FromQuery] int? take) =>
    TypedResults.Ok(
        // TODO better use a dedicated response type instead of exposing internal domain types
        new { RecentOrders = await orders.Recent(take ?? 1) }  
    );
}
