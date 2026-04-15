namespace FactoryMethod.VirtualConstructor.Api;

public interface IMessageFormatter
{
    string Format(string value);
}

public sealed class JsonFormatter : IMessageFormatter
{
    public string Format(string value) => $"{{ \"message\": \"{value}\" }}";
}

public sealed class HtmlFormatter : IMessageFormatter
{
    public string Format(string value) => $"<p>{value}</p>";
}

public abstract class MessageCreator
{
    public string Compose(string value) => CreateFormatter().Format(value);
    protected abstract IMessageFormatter CreateFormatter();
}

public sealed class JsonMessageCreator : MessageCreator
{
    protected override IMessageFormatter CreateFormatter() => new JsonFormatter();
}

public sealed class HtmlMessageCreator : MessageCreator
{
    protected override IMessageFormatter CreateFormatter() => new HtmlFormatter();
}

public static class VirtualConstructorDemo
{
    public static object Create()
    {
        MessageCreator creator = new JsonMessageCreator();
        return new
        {
            Pattern = "Factory Method",
            Variant = "Virtual Constructor",
            Output = creator.Compose("Hello from the factory method pattern")
        };
    }
}
