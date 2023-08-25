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

public sealed class ChaosOrderStore : OderStore
{
    private readonly OderStore _store;
    public ChaosOrderStore(OderStore store) => _store = store;

    private sealed class ChaosMonkeyException : Exception
    {
        public ChaosMonkeyException(string message) : base(message) { }
    }
    
    public async Task Save(Order order)
    {
        if (new Random().Next(0, 10) == 0)
        {
            throw new ChaosMonkeyException("Chaos monkey strikes again!");
        }

        await _store.Save(order);
    }
    
    public async Task<IReadOnlyList<Order>> Recent(int take)
    {
        if (new Random().Next(0, 10) == 0)
        {
            throw new ChaosMonkeyException("Chaos monkey strikes again!");
        }

        var orders = await _store.Recent(take);
        return orders;
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
