using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Orders;

public static class ListOrders
{
    public static Delegate HandleRequest => async (OderStore orders, [FromQuery] int? take) =>
    TypedResults.Ok(
        new { RecentOrders = await orders.Recent(take ?? 1) }
    );
}
