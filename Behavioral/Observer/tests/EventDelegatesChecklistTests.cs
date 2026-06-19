namespace Behavioral.Observer.UnitTests;

using Behavioral.Observer.EventDelegates.Api;

/// <summary>
/// Test checklist for Event Delegates variant:
/// ✓ Observers react without subject knowing concrete details
/// ✓ Handlers can be attached/detached without changing subject internals
/// ✓ Notifications are testable with fake handlers
/// ✓ Handlers are unsubscribed when lifetimes end
/// ✓ Handler order is explicit when order matters
/// </summary>
public class EventDelegatesChecklistTests
{
    [Test]
    public void Publish_NotifiesHandlersWithoutKnowingConcreteHandlerDetails()
    {
        var ticker = new StockTicker("DOTNET");
        var received = new List<string>();

        ticker.PriceChanged += price => received.Add($"email:{price}");
        ticker.PriceChanged += price => received.Add($"dashboard:{price}");

        _ = ticker.Publish("$123.45");

        Assert.That(received, Is.EqualTo(new[] { "email:$123.45", "dashboard:$123.45" }));
    }

    [Test]
    public void SubscribeAndDispose_UnsubscribesHandlerWhenLifetimeEnds()
    {
        var ticker = new StockTicker("DOTNET");
        var received = new List<string>();

        using (ticker.Subscribe(price => received.Add($"ephemeral:{price}")))
        {
            _ = ticker.Publish("$111.00");
        }

        _ = ticker.Publish("$222.00");

        Assert.That(received, Is.EqualTo(new[] { "ephemeral:$111.00" }));
    }

    [Test]
    public void Notifications_AreTestableWithInMemoryHandlers()
    {
        var ticker = new StockTicker("DOTNET");
        var captured = new List<string>();
        ticker.PriceChanged += captured.Add;

        _ = ticker.Publish("$150.00");

        Assert.That(captured, Is.EqualTo(new[] { "$150.00" }));
    }

    [Test]
    public void HandlerOrder_IsExplicit_WhenOrderMatters()
    {
        var ticker = new StockTicker("DOTNET");
        var callOrder = new List<string>();

        ticker.PriceChanged += _ => callOrder.Add("first");
        ticker.PriceChanged += _ => callOrder.Add("second");
        ticker.PriceChanged += _ => callOrder.Add("third");

        _ = ticker.Publish("$199.00");

        Assert.That(callOrder, Is.EqualTo(new[] { "first", "second", "third" }));
    }

    [Test]
    public void EventHandlers_CanBeDetached_WithoutTickerChanges()
    {
        var ticker = new StockTicker("DOTNET");
        var calls = 0;

        void Handler(string _) => calls++;

        ticker.PriceChanged += Handler;
        ticker.PriceChanged -= Handler;

        _ = ticker.Publish("$101.00");

        Assert.That(calls, Is.EqualTo(0));
    }
}
