namespace Behavioral.Mediator.CentralizedMediator.Api;

public interface IMediator
{
    string Send(string sender, string message);
}

public sealed class ChatRoom : IMediator
{
    private readonly List<string> messages = [];

    public string Send(string sender, string message)
    {
        var entry = $"{sender}: {message}";
        messages.Add(entry);
        return entry;
    }

    public IReadOnlyList<string> History => messages;
}

public sealed class Colleague(string name, IMediator mediator)
{
    public string Name { get; } = name;

    public string Say(string message) => mediator.Send(Name, message);
}

public sealed class HierarchicalMediator
{
    private readonly ChatRoom parent = new();
    private readonly Dictionary<string, ChatRoom> children = new();

    public ChatRoom GetTeam(string teamName)
    {
        if (!children.TryGetValue(teamName, out var room))
        {
            room = new ChatRoom();
            children[teamName] = room;
        }

        return room;
    }

    public string Route(string teamName, string sender, string message)
    {
        var local = GetTeam(teamName).Send(sender, message);
        parent.Send($"{teamName}/hub", message);
        return local;
    }
}

public static class MediatorDemo
{
    public static object Create()
    {
        var chatRoom = new ChatRoom();
        var alice = new Colleague("Alice", chatRoom);
        var bob = new Colleague("Bob", chatRoom);

        var hierarchical = new HierarchicalMediator();
        var teamMessage = hierarchical.Route("Support", "Sophie", "Ticket reassigned");

        _ = alice.Say("Hello Bob");
        _ = bob.Say("Hi Alice");

        return new
        {
            Pattern = "Mediator",
            Centralized = chatRoom.History,
            Hierarchical = new
            {
                Team = hierarchical.GetTeam("Support").History,
                Routed = teamMessage
            }
        };
    }
}
