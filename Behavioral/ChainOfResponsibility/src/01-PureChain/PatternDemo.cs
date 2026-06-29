namespace Behavioral.ChainOfResponsibility.PureChain.Api;

public sealed record SupportRequest(string Category, string Message);

public interface IRequestHandler
{
    string? Handle(SupportRequest request);
}

public sealed class SingleHandler(string name) : IRequestHandler
{
    public string Handle(SupportRequest request) => $"{name} handled {request.Category}: {request.Message}";
}

public sealed class ValidationHandler : IRequestHandler
{
    public string? Handle(SupportRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return null;
        }

        return $"Validated {request.Category}";
    }
}

public sealed class PureChain(params IRequestHandler[] handlers)
{
    public string Handle(SupportRequest request)
    {
        foreach (var handler in handlers)
        {
            var result = handler.Handle(request);

            if (result is not null)
            {
                return result;
            }
        }

        return "Request unhandled";
    }
}

public static class ChainDemo
{
    public static object Create()
    {
        var request = new SupportRequest("Billing", "Refund request");
        var single = new SingleHandler("BillingAgent");
        var chain = new PureChain(new ValidationHandler(), new SingleHandler("EscalationDesk"));

        return new
        {
            Pattern = "Chain of Responsibility",
            DirectHandler = single.Handle(request),
            PureChain = chain.Handle(request)
        };
    }
}
