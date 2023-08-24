using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace WebShop.Orders;

public interface OderStore
{
    Task Save(Order order);
    Task<IReadOnlyList<Order>> Recent(int take);
}

public sealed record Order(Guid Id, DateTimeOffset Timestamp, IReadOnlyList<Order.Item> Items)
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

    public Task<IReadOnlyList<Order>> Recent(int take) =>
        Task.FromResult<IReadOnlyList<Order>>(
            _orders.Values.OrderByDescending(o => o.Timestamp).ToImmutableList()
        );
}
