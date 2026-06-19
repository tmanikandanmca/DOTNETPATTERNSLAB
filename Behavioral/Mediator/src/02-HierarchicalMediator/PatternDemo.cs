namespace Behavioral.Mediator.HierarchicalMediator.Api;

public interface ITeamColleague
{
    string Name { get; }
    void ReceiveTeam(string sender, string message);
}

public interface IParentColleague
{
    string Name { get; }
    void ReceiveEscalation(string team, string message);
}

public sealed class TeamMediator(string name)
{
    private readonly List<ITeamColleague> colleagues = [];
    private readonly List<string> localFlow = [];

    public string Name { get; } = name;
    public IReadOnlyList<string> LocalFlow => localFlow;

    public void Register(ITeamColleague colleague) => colleagues.Add(colleague);

    public void Coordinate(string sender, string message)
    {
        foreach (var colleague in colleagues.Where(c => !string.Equals(c.Name, sender, StringComparison.Ordinal)))
        {
            colleague.ReceiveTeam(sender, message);
            localFlow.Add($"{Name}:{sender}->{colleague.Name}:{message}");
        }
    }
}

public sealed class ParentMediator(string name)
{
    private readonly List<IParentColleague> leaders = [];
    private readonly List<string> escalationFlow = [];

    public string Name { get; } = name;

    public void Register(IParentColleague leader) => leaders.Add(leader);

    public void Escalate(string team, string message)
    {
        foreach (var leader in leaders)
        {
            leader.ReceiveEscalation(team, message);
            escalationFlow.Add($"{team}->{leader.Name}:{message}");
        }
    }

    public IReadOnlyList<string> EscalationFlow => escalationFlow;
}

public class TeamMember(string name) : ITeamColleague
{
    public string Name { get; } = name;

    public List<string> Inbox { get; } = [];

    public virtual void ReceiveTeam(string sender, string message)
    {
        Inbox.Add($"team from {sender}: {message}");
    }
}

public class LeadershipMember(string name) : IParentColleague
{
    public string Name { get; } = name;

    public List<string> Escalations { get; } = [];

    public virtual void ReceiveEscalation(string team, string message)
    {
        Escalations.Add($"{team}: {message}");
    }
}

public static class HierarchicalMediatorDemo
{
    public static object Create()
    {
        var parent = new ParentMediator("OperationsHub");
        var support = new TeamMediator("Support");
        var billing = new TeamMediator("Billing");

        var supportAgent = new TeamMember("Sophie");
        var supportLead = new TeamMember("Noah");
        support.Register(supportAgent);
        support.Register(supportLead);

        var billingAgent = new TeamMember("Ben");
        var billingLead = new TeamMember("Maya");
        billing.Register(billingAgent);
        billing.Register(billingLead);

        var director = new LeadershipMember("Director");
        parent.Register(director);

        support.Coordinate("Sophie", "Ticket opened");
        billing.Coordinate("Ben", "Invoice mismatch");

        parent.Escalate(support.Name, "Needs manager approval");

        return new
        {
            Pattern = "Mediator",
            Variant = "Hierarchical Mediator",
            SupportFlow = support.LocalFlow,
            BillingFlow = billing.LocalFlow,
            EscalationFlow = parent.EscalationFlow,
            DirectorInbox = director.Escalations
        };
    }
}
