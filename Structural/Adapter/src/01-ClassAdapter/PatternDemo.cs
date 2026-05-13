namespace Structural.Adapter.ClassAdapter.Api;

public class LegacyAlertWriter
{
    public string WriteLegacy(string category, string message) => $"[{category.ToUpperInvariant()}] {message}";
}

public interface IAlertPublisher
{
    string Publish(string message);
}

public sealed class ClassAdapterPublisher : LegacyAlertWriter, IAlertPublisher
{
    public string Publish(string message) => WriteLegacy("application", message);
}

public static class PatternDemo
{
    public static object Create()
    {
        IAlertPublisher publisher = new ClassAdapterPublisher();
        var alert = publisher.Publish("Payment settled");

        return new
        {
            Pattern = "Adapter",
            Variant = "Class Adapter",
            AdaptedOutput = alert
        };
    }
}
