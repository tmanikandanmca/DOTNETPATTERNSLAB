namespace Behavioral.ChainOfResponsibility.PureChain.Api;

public sealed record SupportRequest(string Category, string Message);

public interface IRequestHandler
{
    string Handle(SupportRequest request);
}

public sealed class SingleHandler(string name) : IRequestHandler
{
    public string Handle(SupportRequest request) => $"{name} handled {request.Category}: {request.Message}";
}

public sealed class ValidationHandler : IRequestHandler
{
    public string Handle(SupportRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return "Request rejected";
        }

        return $"Validated {request.Category}";
    }
}

public sealed class CascadingChain(params IRequestHandler[] handlers)
{
    public IReadOnlyList<string> Handle(SupportRequest request) => handlers
        .Select(handler => handler.Handle(request))
        .ToArray();
}

public static class ChainDemo
{
    public static object Create()
    {
        var request = new SupportRequest("Billing", "Refund request");
        var single = new SingleHandler("BillingAgent");
        var chain = new CascadingChain(new ValidationHandler(), new SingleHandler("EscalationDesk"));

        return new
        {
            Pattern = "Chain of Responsibility",
            PureChain = single.Handle(request),
            CascadingChain = chain.Handle(request)
        };
    }
}
