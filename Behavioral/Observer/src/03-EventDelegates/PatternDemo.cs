namespace Behavioral.Observer.EventDelegates.Api;

public sealed class StockTicker
{
    public event Action<string>? PriceChanged;

    public string Symbol { get; }

    public StockTicker(string symbol) => Symbol = symbol;

    public IDisposable Subscribe(Action<string> handler)
    {
        PriceChanged += handler;
        return new Subscription(() => PriceChanged -= handler);
    }

    public IReadOnlyList<string> Publish(string price)
    {
        var log = new List<string>();
        PriceChanged?.Invoke(price);
        log.Add($"{Symbol}: {price}");
        return log;
    }

    private sealed class Subscription(Action unsubscribe) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            unsubscribe();
            _disposed = true;
        }
    }
}

public static class EventDelegatesDemo
{
    public static object Create()
    {
        var ticker = new StockTicker("DOTNET");
        var notifications = new List<string>();

        ticker.PriceChanged += price => notifications.Add($"Email alert: {price}");
        ticker.PriceChanged += price => notifications.Add($"Dashboard update: {price}");

        _ = ticker.Publish("$123.45");

        return new
        {
            Pattern = "Observer",
            Variant = "Event Delegates",
            Notifications = notifications
        };
    }
}
