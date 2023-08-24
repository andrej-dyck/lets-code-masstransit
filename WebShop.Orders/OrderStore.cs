using System.Collections.Concurrent;

namespace WebShop.Orders;

public interface OderStore
{
    Task Save(Order order);
}

public sealed record Order(Guid Id, IReadOnlyList<Order.Item> Items)
{
    public sealed record Item(string Sku, string Name, int Amount, decimal Price)
    {
        public decimal Total { get; } = Price * Amount;
    }
}

public sealed class OrderInMemoryStore : OderStore
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task Save(Order order)
    {
        _orders.TryAdd(order.Id, order);
        return Task.CompletedTask;
    }
}
