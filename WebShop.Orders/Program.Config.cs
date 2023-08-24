using Microsoft.Extensions.DependencyInjection;

namespace WebShop.Orders;

public static class ProgramConfig
{
    public static IServiceCollection AddOrdersFeature(this IServiceCollection @this) =>
        @this.AddSingleton<OderStore, OrderInMemoryStore>();
}
