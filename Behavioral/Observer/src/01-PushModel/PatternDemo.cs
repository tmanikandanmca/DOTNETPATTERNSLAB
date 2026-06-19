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

    public bool Detach(IObserver observer) => _observers.Remove(observer);

    public IReadOnlyCollection<string> Publish(string state) =>
        _observers.Select(observer => observer.Update(state)).ToArray();
}

public static class ObserverDemo
{
    public static object Create()
    {
        var pushSubject = new PushSubject();
        pushSubject.Attach(new PushObserver("Email"));
        pushSubject.Attach(new PushObserver("Sms"));

        return new
        {
            Pattern = "Observer",
            Variant = "Push Model",
            Notifications = pushSubject.Publish("Order placed")
        };
    }
}
