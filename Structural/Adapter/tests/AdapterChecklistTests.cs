using Structural.Adapter.ClassAdapter.Api;
using Structural.Adapter.ObjectAdapter.Api;

namespace Structural.Adapter.Tests;

public class AdapterChecklistTests
{
    [Test]
    public void Client_CallsOnlyTargetInterfaceMethods_ClassAdapter()
    {
        IAlertPublisher publisher = new ClassAdapterPublisher();

        var output = publisher.Publish("Payment settled");

        Assert.That(output, Is.EqualTo("[APPLICATION] Payment settled"));
    }

    [Test]
    public void Client_CallsOnlyTargetInterfaceMethods_ObjectAdapter()
    {
        INotificationPort port = new ObjectAdapterNotifier(new LegacyNotifier(), "orders.created");

        var output = port.Notify("{ \"id\": 42 }");

        Assert.That(output, Is.EqualTo("topic=orders.created; payload={ \"id\": 42 }"));
    }

    [Test]
    public void ClassAdapter_ForwardsDataCorrectly_ToLegacyAdaptee()
    {
        var adapter = new ClassAdapterPublisher();

        var output = adapter.Publish("Invoice generated");

        Assert.That(output, Is.EqualTo("[APPLICATION] Invoice generated"));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase("order#42@high-priority")]
    [TestCase("Multiline\nPayload")]
    public void ClassAdapter_HandlesEdgeCaseInputs_Clearly(string message)
    {
        IAlertPublisher publisher = new ClassAdapterPublisher();

        var output = publisher.Publish(message);

        Assert.That(output, Is.EqualTo($"[APPLICATION] {message}"));
    }

    [Test]
    public void ObjectAdapter_ForwardsDataCorrectly_ToLegacyAdaptee()
    {
        var legacy = new RecordingLegacyNotifier();
        INotificationPort adapter = new ObjectAdapterNotifier(legacy, "orders.created");

        var output = adapter.Notify("{ \"id\": 42 }");

        Assert.Multiple(() =>
        {
            Assert.That(legacy.CallCount, Is.EqualTo(1));
            Assert.That(legacy.LastTopic, Is.EqualTo("orders.created"));
            Assert.That(legacy.LastPayload, Is.EqualTo("{ \"id\": 42 }"));
            Assert.That(output, Is.EqualTo("topic=orders.created; payload={ \"id\": 42 }"));
        });
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase("{\"id\":42,\"tags\":[\"new\",\"vip\"]}")]
    public void ObjectAdapter_HandlesEdgeCaseInputs_Clearly(string payload)
    {
        INotificationPort port = new ObjectAdapterNotifier(new LegacyNotifier(), "orders.created");

        var output = port.Notify(payload);

        Assert.That(output, Is.EqualTo($"topic=orders.created; payload={payload}"));
    }

    [Test]
    public void ObjectAdapter_WhenAdapteeThrows_PropagatesClearError()
    {
        INotificationPort port = new ObjectAdapterNotifier(new ThrowingLegacyNotifier(), "orders.created");

        var ex = Assert.Throws<InvalidOperationException>(() => port.Notify("{ \"id\": 42 }"));

        Assert.That(ex!.Message, Is.EqualTo("Legacy notifier failure for topic: orders.created"));
    }

    [Test]
    public void ClientCode_CanReplaceAlertPublisherAdapter_WithoutChanges()
    {
        var classAdapter = (IAlertPublisher)new ClassAdapterPublisher();
        var replacementAdapter = (IAlertPublisher)new PrefixingAlertPublisher("[APPLICATION] ");

        var first = SendAlert(classAdapter, "Payment settled");
        var second = SendAlert(replacementAdapter, "Payment settled");

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.EqualTo("[APPLICATION] Payment settled"));
            Assert.That(second, Is.EqualTo("[APPLICATION] Payment settled"));
        });
    }

    [Test]
    public void ClientCode_CanReplaceNotificationAdapter_WithoutChanges()
    {
        var standardAdapter = (INotificationPort)new ObjectAdapterNotifier(new LegacyNotifier(), "orders.created");
        var replacementAdapter = (INotificationPort)new PrefixingNotificationAdapter("topic=orders.created; payload=");

        var first = SendNotification(standardAdapter, "{ \"id\": 42 }");
        var second = SendNotification(replacementAdapter, "{ \"id\": 42 }");

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.EqualTo("topic=orders.created; payload={ \"id\": 42 }"));
            Assert.That(second, Is.EqualTo("topic=orders.created; payload={ \"id\": 42 }"));
        });
    }

    private static string SendAlert(IAlertPublisher publisher, string message) => publisher.Publish(message);

    private static string SendNotification(INotificationPort port, string payload) => port.Notify(payload);

    private sealed class RecordingLegacyNotifier : ILegacyNotifier
    {
        public int CallCount { get; private set; }
        public string LastTopic { get; private set; } = string.Empty;
        public string LastPayload { get; private set; } = string.Empty;

        public string SendLegacy(string topic, string payload)
        {
            CallCount++;
            LastTopic = topic;
            LastPayload = payload;
            return $"topic={topic}; payload={payload}";
        }
    }

    private sealed class ThrowingLegacyNotifier : ILegacyNotifier
    {
        public string SendLegacy(string topic, string payload)
            => throw new InvalidOperationException($"Legacy notifier failure for topic: {topic}");
    }

    private sealed class PrefixingAlertPublisher : IAlertPublisher
    {
        private readonly string _prefix;

        public PrefixingAlertPublisher(string prefix)
        {
            _prefix = prefix;
        }

        public string Publish(string message) => $"{_prefix}{message}";
    }

    private sealed class PrefixingNotificationAdapter : INotificationPort
    {
        private readonly string _prefix;

        public PrefixingNotificationAdapter(string prefix)
        {
            _prefix = prefix;
        }

        public string Notify(string payload) => $"{_prefix}{payload}";
    }
}
