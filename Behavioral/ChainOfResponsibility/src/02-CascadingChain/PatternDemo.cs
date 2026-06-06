namespace Behavioral.ChainOfResponsibility.CascadingChain.Api;

public sealed record PipelineRequest(string Payload);

public interface IHandler
{
    string Handle(string input);
}

public sealed class TrimHandler : IHandler
{
    public string Handle(string input) => input.Trim();
}

public sealed class UpperCaseHandler : IHandler
{
    public string Handle(string input) => input.ToUpperInvariant();
}

public sealed class SuffixHandler(string suffix) : IHandler
{
    public string Handle(string input) => input + suffix;
}

public static class CascadingChainDemo
{
    public static object Create()
    {
        var handlers = new IHandler[] { new TrimHandler(), new UpperCaseHandler(), new SuffixHandler("_DONE") };
        var current = "  request  ";
        var trace = new List<string> { current };

        foreach (var handler in handlers)
        {
            current = handler.Handle(current);
            trace.Add(current);
        }

        return new
        {
            Pattern = "Chain of Responsibility",
            Variant = "Cascading Chain",
            Trace = trace
        };
    }
}
