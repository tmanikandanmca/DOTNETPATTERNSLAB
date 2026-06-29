namespace Behavioral.ChainOfResponsibility.UnitTests;

using Behavioral.ChainOfResponsibility.PureChain.Api;

/// <summary>
/// Test checklist for Pure Chain variant:
/// ✓ Handler classes depend on IRequestHandler abstractions, not caller internals
/// ✓ Chain order is test-covered for expected outcomes
/// ✓ Pure chains stop once a request is handled
/// ✓ Each handler is unit testable in isolation
/// </summary>
public class PureChainChecklistTests
{
    [Test]
    public void ClientCode_UsesIRequestHandlerAbstraction_WithoutKnowingConcreteInternals()
    {
        IRequestHandler handler = new SingleHandler("BillingAgent");
        var request = new SupportRequest("Billing", "Refund request");

        var result = handler.Handle(request);

        Assert.That(result, Is.EqualTo("BillingAgent handled Billing: Refund request"));
    }

    [Test]
    public void ChainOrder_IsCovered_ForExpectedOutcome()
    {
        var calls = new List<string>();
        var chain = new PureChain(
            new RecordingRejectingHandler("first", calls),
            new RecordingHandlingHandler("second", calls),
            new RecordingRejectingHandler("third", calls));

        var result = chain.Handle(new SupportRequest("Billing", "Refund request"));

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo("second handled Billing"));
            Assert.That(calls, Is.EqualTo(new[] { "first", "second" }));
        });
    }

    [Test]
    public void PureChain_StopsOnceARequestIsHandled()
    {
        var calls = new List<string>();
        var chain = new PureChain(
            new RecordingRejectingHandler("alpha", calls),
            new RecordingHandlingHandler("beta", calls),
            new RecordingRejectingHandler("gamma", calls));

        var result = chain.Handle(new SupportRequest("Support", "Need help"));

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo("beta handled Support"));
            Assert.That(calls, Is.EqualTo(new[] { "alpha", "beta" }));
        });
    }

    [Test]
    public void ValidationHandler_IsUnitTestableInIsolation()
    {
        var handler = new ValidationHandler();

        var invalidResult = handler.Handle(new SupportRequest("Billing", "   "));
        var validResult = handler.Handle(new SupportRequest("Billing", "Refund request"));

        Assert.Multiple(() =>
        {
            Assert.That(invalidResult, Is.Null);
            Assert.That(validResult, Is.EqualTo("Validated Billing"));
        });
    }

    [Test]
    public void SingleHandler_IsUnitTestableInIsolation()
    {
        var handler = new SingleHandler("EscalationDesk");

        var result = handler.Handle(new SupportRequest("Billing", "Refund request"));

        Assert.That(result, Is.EqualTo("EscalationDesk handled Billing: Refund request"));
    }

    private sealed class RecordingRejectingHandler(string name, IList<string> calls) : IRequestHandler
    {
        public string? Handle(SupportRequest request)
        {
            calls.Add(name);
            return null;
        }
    }

    private sealed class RecordingHandlingHandler(string name, IList<string> calls) : IRequestHandler
    {
        public string? Handle(SupportRequest request)
        {
            calls.Add(name);
            return $"{name} handled {request.Category}";
        }
    }
}