namespace Structural.Bridge.RefinedAbstractionVsConcreteImplementor.Api;

public interface IChannel
{
    string Send(string message);
}

public sealed class EmailChannel : IChannel
{
    public string Send(string message) => $"EMAIL::{message}";
}

public sealed class SmsChannel : IChannel
{
    public string Send(string message) => $"SMS::{message}";
}

public abstract class Notification
{
    protected readonly IChannel Channel;

    protected Notification(IChannel channel) => Channel = channel;

    public abstract string Dispatch(string message);
}

public sealed class StandardNotification : Notification
{
    public StandardNotification(IChannel channel) : base(channel) { }

    public override string Dispatch(string message) => Channel.Send(message);
}

public sealed class PriorityNotification : Notification
{
    public PriorityNotification(IChannel channel) : base(channel) { }

    public override string Dispatch(string message) => Channel.Send($"[PRIORITY] {message}");
}

public static class PatternDemo
{
    public static object Create()
    {
        Notification standardEmail = new StandardNotification(new EmailChannel());
        Notification prioritySms = new PriorityNotification(new SmsChannel());

        return new
        {
            Pattern = "Bridge",
            Variant = "Refined Abstraction vs Concrete Implementor",
            Standard = standardEmail.Dispatch("Build succeeded"),
            Priority = prioritySms.Dispatch("API down")
        };
    }
}
