namespace Behavioral.Observer.UnitTests;

using Behavioral.Observer.PullModel.Api;

/// <summary>
/// Test checklist for Pull Model variant:
/// ✓ Observers react without sensor knowing concrete details
/// ✓ Attaching or detaching observers does not require sensor changes
/// ✓ Notifications are testable with fake observers
/// ✓ Observer order is explicit when order matters
/// </summary>
public class PullModelChecklistTests
{
    [Test]
    public void SetTemperature_NotifiesObserversThroughAbstraction()
    {
        var sensor = new TemperatureSensor();
        var first = new FakePullObserver("Dashboard");
        var second = new FakePullObserver("Alerting");

        sensor.Attach(first);
        sensor.Attach(second);

        var result = sensor.SetTemperature(31);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(new[]
            {
                "Dashboard pulled 31 C",
                "Alerting pulled 31 C"
            }));
            Assert.That(first.LastObservedTemperature, Is.EqualTo(31));
            Assert.That(second.LastObservedTemperature, Is.EqualTo(31));
        });
    }

    [Test]
    public void AttachAndDetach_ChangeSubscribersWithoutSensorModification()
    {
        var sensor = new TemperatureSensor();
        var active = new FakePullObserver("Active");
        var removed = new FakePullObserver("Removed");

        sensor.Attach(active);
        sensor.Attach(removed);

        var detached = sensor.Detach(removed);
        var result = sensor.SetTemperature(22);

        Assert.Multiple(() =>
        {
            Assert.That(detached, Is.True);
            Assert.That(result, Is.EqualTo(new[] { "Active pulled 22 C" }));
            Assert.That(active.LastObservedTemperature, Is.EqualTo(22));
            Assert.That(removed.LastObservedTemperature, Is.Null);
        });
    }

    [Test]
    public void PullModel_ObserversReadStateFromSubjectAfterNotification()
    {
        var sensor = new TemperatureSensor();
        var fake = new FakePullObserver("Probe");
        sensor.Attach(fake);

        _ = sensor.SetTemperature(28);

        Assert.That(fake.LastObservedTemperature, Is.EqualTo(sensor.TemperatureCelsius));
    }

    [Test]
    public void NotificationOrder_IsExplicit_WhenOrderMatters()
    {
        var sensor = new TemperatureSensor();
        var callOrder = new List<string>();

        sensor.Attach(new OrderedPullObserver("first", callOrder));
        sensor.Attach(new OrderedPullObserver("second", callOrder));
        sensor.Attach(new OrderedPullObserver("third", callOrder));

        _ = sensor.SetTemperature(18);

        Assert.That(callOrder, Is.EqualTo(new[] { "first", "second", "third" }));
    }

    private sealed class FakePullObserver(string name) : IPullObserver
    {
        public string Name { get; } = name;

        public int? LastObservedTemperature { get; private set; }

        public string Refresh(TemperatureSensor sensor)
        {
            LastObservedTemperature = sensor.TemperatureCelsius;
            return $"{Name} pulled {sensor.TemperatureCelsius} C";
        }
    }

    private sealed class OrderedPullObserver(string name, IList<string> callOrder) : IPullObserver
    {
        public string Name { get; } = name;

        public string Refresh(TemperatureSensor sensor)
        {
            callOrder.Add(Name);
            return $"{Name} pulled {sensor.TemperatureCelsius} C";
        }
    }
}
