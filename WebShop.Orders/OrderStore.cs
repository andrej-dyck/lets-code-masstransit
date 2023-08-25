using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace WebShop.Orders;

public interface OrderStore
{
    Task Save(Order order);
    Task<IReadOnlyList<Order>> Recent(int take);
    Task<Order?> ById(Guid id);
}

public sealed record Order(Guid Id, DateTimeOffset Timestamp, IReadOnlyList<Order.Item> Items)
{
    public sealed record Item(string Sku, string Name, int Amount, decimal Price)
    {
        public decimal Total { get; } = Price * Amount;
    }
}

public sealed class ChaosOrderStore : OrderStore
{
    private readonly OrderStore _store;
    public ChaosOrderStore(OrderStore store) => _store = store;

    public async Task Save(Order order)
    {
        FailRandomly();
        await _store.Save(order);
    }

    public Task<IReadOnlyList<Order>> Recent(int take)
    {
        FailRandomly();
        return _store.Recent(take);
    }

    public Task<Order?> ById(Guid id) => _store.ById(id);

    private static void FailRandomly()
    {
        if (new Random().Next(0, 5) == 0) throw new ChaosMonkey();
    }

    private sealed class ChaosMonkey : Exception
    {
        public ChaosMonkey() : base("Chaos monkey strikes again!") { }
    }
}

public sealed class OrderInMemoryStore : OrderStore
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

    public Task<Order?> ById(Guid id) => 
        Task.FromResult(_orders.GetValueOrDefault(id));
}
