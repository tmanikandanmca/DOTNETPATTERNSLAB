namespace Structural.Proxy.SmartProxy.Api;

public interface IStockService
{
    int GetQuantity(string sku);
}

public sealed class StockService : IStockService
{
    public static int Calls;

    public int GetQuantity(string sku)
    {
        Calls++;
        return sku switch
        {
            "kbd-01" => 18,
            "mse-02" => 7,
            _ => 0
        };
    }
}

public sealed class SmartStockProxy : IStockService
{
    private readonly IStockService _inner;
    private readonly Dictionary<string, int> _cache = new(StringComparer.OrdinalIgnoreCase);

    public SmartStockProxy(IStockService inner) => _inner = inner;

    public int CacheHits { get; private set; }

    public int GetQuantity(string sku)
    {
        if (_cache.TryGetValue(sku, out var cached))
        {
            CacheHits++;
            return cached;
        }

        var qty = _inner.GetQuantity(sku);
        _cache[sku] = qty;
        return qty;
    }
}

public static class PatternDemo
{
    public static object Create()
    {
        StockService.Calls = 0;
        var proxy = new SmartStockProxy(new StockService());

        var first = proxy.GetQuantity("kbd-01");
        var second = proxy.GetQuantity("kbd-01");
        var third = proxy.GetQuantity("mse-02");

        return new
        {
            Pattern = "Proxy",
            Variant = "Smart Proxy",
            Quantities = new[] { first, second, third },
            BackendCalls = StockService.Calls,
            CacheHits = proxy.CacheHits
        };
    }
}
