namespace Behavioral.Mediator.CentralizedMediator.Api;

public interface IChatMediator
{
    void Register(IChatColleague colleague);
    void Send(string sender, string message);
}

public interface IChatColleague
{
    string Name { get; }
    void Receive(string sender, string message);
}

public sealed class ChatRoomMediator : IChatMediator
{
    private readonly List<IChatColleague> colleagues = [];
    private readonly List<string> deliveryLog = [];

    public void Register(IChatColleague colleague) => colleagues.Add(colleague);

    public void Send(string sender, string message)
    {
        // Order is controlled in mediator so workflow ordering stays centralized.
        foreach (var colleague in colleagues.Where(c => !string.Equals(c.Name, sender, StringComparison.Ordinal)))
        {
            colleague.Receive(sender, message);
            deliveryLog.Add($"{sender}->{colleague.Name}:{message}");
        }
    }

    public IReadOnlyList<string> DeliveryLog => deliveryLog;
}

public class ChatColleague(string name, IChatMediator mediator) : IChatColleague
{
    public string Name { get; } = name;

    public List<string> Inbox { get; } = [];

    public void Send(string message) => mediator.Send(Name, message);

    public virtual void Receive(string sender, string message)
    {
        Inbox.Add($"from {sender}: {message}");
    }
}

public static class MediatorDemo
{
    public static object Create()
    {
        var chatRoom = new ChatRoomMediator();
        var alice = new ChatColleague("Alice", chatRoom);
        var bob = new ChatColleague("Bob", chatRoom);
        var ops = new ChatColleague("Ops", chatRoom);

        chatRoom.Register(alice);
        chatRoom.Register(bob);
        chatRoom.Register(ops);

        alice.Send("Deployment at 17:00");
        bob.Send("Acknowledged");

        return new
        {
            Pattern = "Mediator",
            Variant = "Centralized Mediator",
            Delivery = chatRoom.DeliveryLog,
            BobInbox = bob.Inbox,
            OpsInbox = ops.Inbox
        };
    }
}
