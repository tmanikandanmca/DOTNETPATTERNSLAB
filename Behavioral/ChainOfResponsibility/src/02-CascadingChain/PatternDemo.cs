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

public sealed class CascadingChain(params IHandler[] handlers)
{
    public string Handle(string input, IList<string>? trace = null)
    {
        var current = input;
        trace?.Add(current);

        foreach (var handler in handlers)
        {
            current = handler.Handle(current);
            trace?.Add(current);
        }

        return current;
    }
}

public static class CascadingChainDemo
{
    public static object Create()
    {
        var chain = new CascadingChain(new TrimHandler(), new UpperCaseHandler(), new SuffixHandler("_DONE"));
        var trace = new List<string>();
        var result = chain.Handle("  request  ", trace);

        return new
        {
            Pattern = "Chain of Responsibility",
            Variant = "Cascading Chain",
            Result = result,
            Trace = trace
        };
    }
}
