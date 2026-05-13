namespace Structural.Decorator.SemiTransparentDecorator.Api;

public interface IMessage
{
    string Render();
}

public sealed class SimpleMessage : IMessage
{
    private readonly string _content;

    public SimpleMessage(string content) => _content = content;

    public string Render() => _content;
}

public sealed class TracingDecorator : IMessage
{
    private readonly IMessage _inner;

    public TracingDecorator(IMessage inner)
    {
        _inner = inner;
        TraceId = Guid.NewGuid().ToString("N");
    }

    public string TraceId { get; }

    public string Render() => $"trace={TraceId[..8]}::{_inner.Render()}";
}

public static class PatternDemo
{
    public static object Create()
    {
        IMessage message = new TracingDecorator(new SimpleMessage("invoice generated"));
        var rendered = message.Render();
        var traceId = (message as TracingDecorator)?.TraceId ?? "n/a";

        return new
        {
            Pattern = "Decorator",
            Variant = "Semi-transparent Decorator",
            Rendered = rendered,
            DecoratorSpecificState = traceId
        };
    }
}
