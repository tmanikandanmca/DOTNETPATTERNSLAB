namespace Behavioral.Mediator.UnitTests;

using Behavioral.Mediator.CentralizedMediator.Api;

/// <summary>
/// Test checklist for Centralized Mediator variant:
/// ✓ Colleagues do not call each other directly
/// ✓ Workflow ordering is defined by mediator
/// ✓ Mediator stays focused on one coordination boundary
/// ✓ Orchestration testable with fake colleagues
/// ✓ A colleague can be swapped without changing all peers
/// </summary>
public class CentralizedMediatorChecklistTests
{
    [Test]
    public void Colleagues_CommunicateOnlyThroughMediator()
    {
        var mediator = new ChatRoomMediator();
        var alice = new ChatColleague("Alice", mediator);
        var bob = new ChatColleague("Bob", mediator);

        mediator.Register(alice);
        mediator.Register(bob);

        alice.Send("Deploy at 17:00");

        Assert.Multiple(() =>
        {
            Assert.That(alice.Inbox, Is.Empty);
            Assert.That(bob.Inbox, Is.EqualTo(new[] { "from Alice: Deploy at 17:00" }));
        });
    }

    [Test]
    public void WorkflowOrdering_LivesInMediator_NotInColleagues()
    {
        var mediator = new ChatRoomMediator();
        var alice = new ChatColleague("Alice", mediator);
        var first = new RecordingColleague("First");
        var second = new RecordingColleague("Second");

        mediator.Register(alice);
        mediator.Register(first);
        mediator.Register(second);

        alice.Send("Start rollout");

        Assert.That(mediator.DeliveryLog, Is.EqualTo(new[]
        {
            "Alice->First:Start rollout",
            "Alice->Second:Start rollout"
        }));
    }

    [Test]
    public void Mediator_RemainsWithinSingleCoordinationBoundary_ChatRoomOnly()
    {
        var mediator = new ChatRoomMediator();
        var alice = new ChatColleague("Alice", mediator);
        var bob = new ChatColleague("Bob", mediator);

        mediator.Register(alice);
        mediator.Register(bob);

        bob.Send("Ack");

        Assert.That(mediator.DeliveryLog, Is.EqualTo(new[] { "Bob->Alice:Ack" }));
    }

    [Test]
    public void Orchestration_IsTestableWithFakeColleagues()
    {
        var mediator = new ChatRoomMediator();
        var sender = new ChatColleague("Sender", mediator);
        var fake = new RecordingColleague("Recorder");

        mediator.Register(sender);
        mediator.Register(fake);

        sender.Send("Test ping");

        Assert.Multiple(() =>
        {
            Assert.That(fake.Received, Is.EqualTo(new[] { "from Sender: Test ping" }));
            Assert.That(mediator.DeliveryLog, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void SwappingColleague_DoesNotRequirePeerChanges()
    {
        var mediator = new ChatRoomMediator();
        var alice = new ChatColleague("Alice", mediator);
        var bob = new ChatColleague("Bob", mediator);
        var swapped = new RecordingColleague("Ops");

        mediator.Register(alice);
        mediator.Register(bob);
        mediator.Register(swapped);

        alice.Send("Maintenance window");

        Assert.Multiple(() =>
        {
            Assert.That(bob.Inbox, Is.EqualTo(new[] { "from Alice: Maintenance window" }));
            Assert.That(swapped.Received, Is.EqualTo(new[] { "from Alice: Maintenance window" }));
        });
    }

    private sealed class RecordingColleague(string name) : IChatColleague
    {
        public string Name { get; } = name;

        public List<string> Received { get; } = [];

        public void Receive(string sender, string message)
        {
            Received.Add($"from {sender}: {message}");
        }
    }
}
