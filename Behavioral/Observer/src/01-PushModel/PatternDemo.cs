namespace Observer.PushModel.Api;

public interface IObserver
{
    string Name { get; }
    string Update(string state);
}

public sealed class PushObserver(string name) : IObserver
{
    public string Name { get; } = name;

    public string Update(string state) => $"{Name} received: {state}";
}

public sealed class PushSubject
{
    private readonly List<IObserver> _observers = [];

    public void Attach(IObserver observer) => _observers.Add(observer);

    public IReadOnlyCollection<string> Publish(string state) =>
        _observers.Select(observer => observer.Update(state)).ToArray();
}

public sealed class PullSubject
{
    private readonly List<PullObserver> _observers = [];

    public string State { get; private set; } = "idle";

    public void Attach(PullObserver observer) => _observers.Add(observer);

    public IReadOnlyCollection<string> Publish(string state)
    {
        State = state;
        return _observers.Select(observer => observer.Read(State)).ToArray();
    }
}

public sealed class PullObserver(string name)
{
    public string Name { get; } = name;

    public string Read(string state) => $"{Name} pulled: {state}";
}

public sealed class EventFeed
{
    public event EventHandler<string>? Updated;

    public string Publish(string state)
    {
        Updated?.Invoke(this, state);
        return state;
    }
}

public static class ObserverDemo
{
    public static object Create()
    {
        var pushSubject = new PushSubject();
        pushSubject.Attach(new PushObserver("Email"));
        pushSubject.Attach(new PushObserver("Sms"));

        var pullSubject = new PullSubject();
        pullSubject.Attach(new PullObserver("Cache"));
        pullSubject.Attach(new PullObserver("Dashboard"));

        var eventFeed = new EventFeed();
        var delegateLog = new List<string>();
        eventFeed.Updated += (_, state) => delegateLog.Add($"Delegate handled: {state}");

        return new
        {
            Pattern = "Observer",
            Push = pushSubject.Publish("Order placed"),
            Pull = pullSubject.Publish("Inventory changed"),
            Events = eventFeed.Publish("Stock refreshed"),
            DelegateNotes = delegateLog
        };
    }
}
