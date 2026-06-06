namespace Behavioral.Observer.EventDelegates.Api;

public sealed class StockTicker
{
    public event Action<string>? PriceChanged;

    public string Symbol { get; }

    public StockTicker(string symbol) => Symbol = symbol;

    public IReadOnlyList<string> Publish(string price)
    {
        var log = new List<string>();
        PriceChanged?.Invoke(price);
        log.Add($"{Symbol}: {price}");
        return log;
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
