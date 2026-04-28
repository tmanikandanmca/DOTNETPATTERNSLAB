namespace AbstractFactory.FactoryOfFactories.Api;

public interface INotificationSender
{
    string Channel { get; }
    string Send(string message);
}

public interface IMessageFormatter
{
    string Format(string message);
}

public interface INotificationSuiteFactory
{
    string Family { get; }
    INotificationSender CreateSender();
    IMessageFormatter CreateFormatter();
}

public sealed class EmailSender : INotificationSender
{
    public string Channel => "Email";

    public string Send(string message) => $"EMAIL:{message}";
}

public sealed class EmailFormatter : IMessageFormatter
{
    public string Format(string message) => $"[EMAIL] {message}";
}

public sealed class SmsSender : INotificationSender
{
    public string Channel => "Sms";

    public string Send(string message) => $"SMS:{message}";
}

public sealed class SmsFormatter : IMessageFormatter
{
    public string Format(string message) => $"[SMS] {message}";
}

public sealed class EmailNotificationSuiteFactory : INotificationSuiteFactory
{
    public string Family => "Email";

    public INotificationSender CreateSender() => new EmailSender();

    public IMessageFormatter CreateFormatter() => new EmailFormatter();
}

public sealed class SmsNotificationSuiteFactory : INotificationSuiteFactory
{
    public string Family => "Sms";

    public INotificationSender CreateSender() => new SmsSender();

    public IMessageFormatter CreateFormatter() => new SmsFormatter();
}

public static class NotificationSuiteFactoryProvider
{
    public static INotificationSuiteFactory Create(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentException("Channel must be provided.", nameof(channel));
        }

        return channel.ToLowerInvariant() switch
        {
            "email" => new EmailNotificationSuiteFactory(),
            "sms" => new SmsNotificationSuiteFactory(),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, "Unsupported channel. Supported values are 'email' and 'sms'.")
        };
    }
}

public sealed class NotificationService
{
    private readonly INotificationSuiteFactory _factory;

    public NotificationService(INotificationSuiteFactory factory)
    {
        _factory = factory;
    }

    public object Send(string message)
    {
        var formatter = _factory.CreateFormatter();
        var sender = _factory.CreateSender();
        var payload = formatter.Format(message);

        return new
        {
            Family = _factory.Family,
            Sender = sender.Channel,
            Payload = sender.Send(payload)
        };
    }
}

public static class FactoryOfFactoriesDemo
{
    public static object Create()
    {
        var factory = NotificationSuiteFactoryProvider.Create("email");
        var service = new NotificationService(factory);

        return new
        {
            Pattern = "Abstract Factory",
            Variant = "Factory of Factories",
            Channel = factory.Family,
            Example = service.Send("deployment complete")
        };
    }
}
