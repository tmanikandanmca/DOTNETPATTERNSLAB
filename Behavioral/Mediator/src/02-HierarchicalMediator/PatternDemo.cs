namespace Behavioral.Mediator.HierarchicalMediator.Api;

public sealed class TeamMediator(string name)
{
    private readonly List<string> messages = [];

    public string Name { get; } = name;
    public IReadOnlyList<string> Messages => messages;

    public void Publish(string sender, string message) => messages.Add($"{sender}: {message}");
}

public sealed class ParentMediator
{
    private readonly List<string> escalations = [];

    public void Escalate(string team, string message) => escalations.Add($"{team} -> {message}");

    public IReadOnlyList<string> Escalations => escalations;
}

public static class HierarchicalMediatorDemo
{
    public static object Create()
    {
        var parent = new ParentMediator();
        var support = new TeamMediator("Support");
        var billing = new TeamMediator("Billing");

        support.Publish("Sophie", "Ticket opened");
        billing.Publish("Ben", "Invoice mismatch");

        parent.Escalate(support.Name, "Needs manager approval");

        return new
        {
            Pattern = "Mediator",
            Variant = "Hierarchical Mediator",
            Support = support.Messages,
            Billing = billing.Messages,
            Escalations = parent.Escalations
        };
    }
}
