namespace Structural.Adapter.ObjectAdapter.Api;

public interface ILegacyNotifier
{
    string SendLegacy(string topic, string payload);
}

public sealed class LegacyNotifier : ILegacyNotifier
{
    public string SendLegacy(string topic, string payload) => $"topic={topic}; payload={payload}";
}

public interface INotificationPort
{
    string Notify(string payload);
}

public sealed class ObjectAdapterNotifier : INotificationPort
{
    private readonly ILegacyNotifier _legacy;
    private readonly string _topic;

    public ObjectAdapterNotifier(ILegacyNotifier legacy, string topic)
    {
        _legacy = legacy;
        _topic = topic;
    }

    public string Notify(string payload) => _legacy.SendLegacy(_topic, payload);
}

public static class PatternDemo
{
    public static object Create()
    {
        var adapter = new ObjectAdapterNotifier(new LegacyNotifier(), "orders.created");
        var notification = adapter.Notify("{ \"id\": 42 }");

        return new
        {
            Pattern = "Adapter",
            Variant = "Object Adapter",
            AdaptedOutput = notification
        };
    }
}
