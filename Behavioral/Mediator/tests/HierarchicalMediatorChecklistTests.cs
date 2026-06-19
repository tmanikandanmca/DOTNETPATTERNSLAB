namespace Behavioral.Mediator.UnitTests;

using Behavioral.Mediator.HierarchicalMediator.Api;

/// <summary>
/// Test checklist for Hierarchical Mediator variant:
/// ✓ Colleagues do not call each other directly
/// ✓ Workflow ordering is defined by mediators (team then parent)
/// ✓ Each mediator focuses on a single coordination boundary
/// ✓ Orchestration testable with fake colleagues and leaders
/// ✓ A colleague can be swapped without changing all peers
/// </summary>
public class HierarchicalMediatorChecklistTests
{
    [Test]
    public void TeamColleagues_DoNotCallEachOtherDirectly_UseTeamMediator()
    {
        var team = new TeamMediator("Support");
        var alice = new TeamMember("Alice");
        var bob = new TeamMember("Bob");

        team.Register(alice);
        team.Register(bob);

        team.Coordinate("Alice", "Ticket assigned");

        Assert.Multiple(() =>
        {
            Assert.That(alice.Inbox, Is.Empty);
            Assert.That(bob.Inbox, Is.EqualTo(new[] { "team from Alice: Ticket assigned" }));
        });
    }

    [Test]
    public void WorkflowOrdering_IsTeamFirst_ThenParentEscalation()
    {
        var team = new TeamMediator("Support");
        var parent = new ParentMediator("Hub");

        var first = new TeamMember("First");
        var second = new TeamMember("Second");
        var director = new LeadershipMember("Director");

        team.Register(first);
        team.Register(second);
        parent.Register(director);

        team.Coordinate("First", "Need approval");
        parent.Escalate(team.Name, "Need approval");

        Assert.Multiple(() =>
        {
            Assert.That(team.LocalFlow, Is.EqualTo(new[] { "Support:First->Second:Need approval" }));
            Assert.That(parent.EscalationFlow, Is.EqualTo(new[] { "Support->Director:Need approval" }));
        });
    }

    [Test]
    public void EachMediator_HasSingleCoordinationBoundary()
    {
        var support = new TeamMediator("Support");
        var billing = new TeamMediator("Billing");
        var parent = new ParentMediator("Hub");

        var supportMember = new TeamMember("S1");
        var billingMember = new TeamMember("B1");
        var leader = new LeadershipMember("L1");

        support.Register(supportMember);
        billing.Register(billingMember);
        parent.Register(leader);

        support.Coordinate("S1", "Support only");
        billing.Coordinate("B1", "Billing only");
        parent.Escalate("Support", "Escalated");

        Assert.Multiple(() =>
        {
            Assert.That(support.LocalFlow, Has.All.StartWith("Support:"));
            Assert.That(billing.LocalFlow, Has.All.StartWith("Billing:"));
            Assert.That(parent.EscalationFlow, Has.All.StartWith("Support->"));
        });
    }

    [Test]
    public void Orchestration_IsTestableWithFakeParticipants()
    {
        var team = new TeamMediator("Support");
        var parent = new ParentMediator("Hub");

        var fakeMember = new RecordingTeamColleague("Recorder");
        var fakeLeader = new RecordingParentColleague("LeadRecorder");

        team.Register(fakeMember);
        parent.Register(fakeLeader);

        team.Coordinate("Agent", "Case opened");
        parent.Escalate(team.Name, "Case opened");

        Assert.Multiple(() =>
        {
            Assert.That(fakeMember.Received, Is.EqualTo(new[] { "team from Agent: Case opened" }));
            Assert.That(fakeLeader.Received, Is.EqualTo(new[] { "Support: Case opened" }));
        });
    }

    [Test]
    public void SwappingTeamColleague_DoesNotRequirePeerChanges()
    {
        var team = new TeamMediator("Support");

        var sender = new TeamMember("Sender");
        var oldPeer = new TeamMember("PeerA");
        var newPeer = new RecordingTeamColleague("PeerB");

        team.Register(sender);
        team.Register(oldPeer);
        team.Register(newPeer);

        team.Coordinate("Sender", "Daily update");

        Assert.Multiple(() =>
        {
            Assert.That(oldPeer.Inbox, Is.EqualTo(new[] { "team from Sender: Daily update" }));
            Assert.That(newPeer.Received, Is.EqualTo(new[] { "team from Sender: Daily update" }));
        });
    }

    private sealed class RecordingTeamColleague(string name) : ITeamColleague
    {
        public string Name { get; } = name;

        public List<string> Received { get; } = [];

        public void ReceiveTeam(string sender, string message)
        {
            Received.Add($"team from {sender}: {message}");
        }
    }

    private sealed class RecordingParentColleague(string name) : IParentColleague
    {
        public string Name { get; } = name;

        public List<string> Received { get; } = [];

        public void ReceiveEscalation(string team, string message)
        {
            Received.Add($"{team}: {message}");
        }
    }
}
