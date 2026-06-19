namespace Behavioral.Observer.UnitTests;

using PushApi = global::Observer.PushModel.Api;

/// <summary>
/// Test checklist for Push Model variant:
/// ✓ Observers react without subject knowing concrete details
/// ✓ Attaching or detaching observers does not require subject changes
/// ✓ Notifications are testable with fake observers
/// ✓ Observer order is explicit when order matters
/// </summary>
public class PushModelChecklistTests
{
    [Test]
    public void Publish_NotifiesObserversWithoutKnowingConcreteDetails()
    {
        var subject = new PushApi.PushSubject();
        var first = new FakePushObserver("Email");
        var second = new FakePushObserver("Audit");

        subject.Attach(first);
        subject.Attach(second);

        var result = subject.Publish("OrderPlaced");

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(new[]
            {
                "Email observed: OrderPlaced",
                "Audit observed: OrderPlaced"
            }));
            Assert.That(first.ReceivedStates, Is.EqualTo(new[] { "OrderPlaced" }));
            Assert.That(second.ReceivedStates, Is.EqualTo(new[] { "OrderPlaced" }));
        });
    }

    [Test]
    public void AttachAndDetach_ChangeSubscribersWithoutSubjectModification()
    {
        var subject = new PushApi.PushSubject();
        var active = new FakePushObserver("Active");
        var removed = new FakePushObserver("Removed");

        subject.Attach(active);
        subject.Attach(removed);

        var detached = subject.Detach(removed);
        var result = subject.Publish("InventoryUpdated");

        Assert.Multiple(() =>
        {
            Assert.That(detached, Is.True);
            Assert.That(result, Is.EqualTo(new[] { "Active observed: InventoryUpdated" }));
            Assert.That(active.ReceivedStates, Is.EqualTo(new[] { "InventoryUpdated" }));
            Assert.That(removed.ReceivedStates, Is.Empty);
        });
    }

    [Test]
    public void Notifications_AreTestableWithFakeObservers()
    {
        var subject = new PushApi.PushSubject();
        var fake = new FakePushObserver("Fake");
        subject.Attach(fake);

        _ = subject.Publish("PaymentCaptured");

        Assert.That(fake.ReceivedStates, Is.EqualTo(new[] { "PaymentCaptured" }));
    }

    [Test]
    public void NotificationOrder_IsExplicit_WhenOrderMatters()
    {
        var subject = new PushApi.PushSubject();
        var callOrder = new List<string>();

        subject.Attach(new OrderedPushObserver("first", callOrder));
        subject.Attach(new OrderedPushObserver("second", callOrder));
        subject.Attach(new OrderedPushObserver("third", callOrder));

        _ = subject.Publish("AnyState");

        Assert.That(callOrder, Is.EqualTo(new[] { "first", "second", "third" }));
    }

    private sealed class FakePushObserver(string name) : PushApi.IObserver
    {
        public string Name { get; } = name;

        public List<string> ReceivedStates { get; } = [];

        public string Update(string state)
        {
            ReceivedStates.Add(state);
            return $"{Name} observed: {state}";
        }
    }

    private sealed class OrderedPushObserver(string name, IList<string> callOrder) : PushApi.IObserver
    {
        public string Name { get; } = name;

        public string Update(string state)
        {
            callOrder.Add(Name);
            return $"{Name}:{state}";
        }
    }
}
