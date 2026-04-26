namespace FactoryMethod.ParameterizedFactory.Api;

public interface INotifier
{
    string Channel { get; }
    string Send(string message);
}

public sealed class EmailNotifier : INotifier
{
    public string Channel => "Email";
    public string Send(string message) => $"Email sent: {message}";
}

public sealed class SmsNotifier : INotifier
{
    public string Channel => "SMS";
    public string Send(string message) => $"SMS sent: {message}";
}

public static class NotifierFactory
{
    public static INotifier Create(string channel, bool highPriority)
    {
        if (!channel.Equals("email", StringComparison.OrdinalIgnoreCase)
            && !channel.Equals("sms", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentOutOfRangeException(
                nameof(channel),
                channel,
                "Unknown channel. Supported values are 'email' and 'sms'.");
        }

        if (highPriority || channel.Equals("sms", StringComparison.OrdinalIgnoreCase))
        {
            return new SmsNotifier();
        }

        return new EmailNotifier();
    }
}

public static class ParameterizedFactoryDemo
{
    public static object Create()
    {
        var notifier = NotifierFactory.Create("email", highPriority: true);
        return new
        {
            Pattern = "Factory Method",
            Variant = "Parameterized Factory",
            Product = notifier.Channel,
            Result = notifier.Send("Deployment completed")
        };
    }
}
