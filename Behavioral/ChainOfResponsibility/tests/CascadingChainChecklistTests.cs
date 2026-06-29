namespace Behavioral.ChainOfResponsibility.UnitTests;

using Behavioral.ChainOfResponsibility.CascadingChain.Api;

/// <summary>
/// Test checklist for Cascading Chain variant:
/// ✓ Handler classes depend on IHandler abstractions, not caller internals
/// ✓ Chain order is test-covered for expected outcomes
/// ✓ Cascading chains preserve handler execution order
/// ✓ Each handler is unit testable in isolation
/// </summary>
public class CascadingChainChecklistTests
{
    [Test]
    public void ClientCode_UsesIHandlerAbstraction_ToComposeThePipeline()
    {
        IHandler handler = new TrimHandler();

        var result = handler.Handle("  request  ");

        Assert.That(result, Is.EqualTo("request"));
    }

    [Test]
    public void ChainOrder_IsCovered_ForExpectedOutcome()
    {
        var calls = new List<string>();
        var chain = new CascadingChain(
            new RecordingHandler("trim", calls, value => value.Trim()),
            new RecordingHandler("upper", calls, value => value.ToUpperInvariant()),
            new RecordingHandler("suffix", calls, value => value + "_DONE"));

        var result = chain.Handle("  request  ");

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo("REQUEST_DONE"));
            Assert.That(calls, Is.EqualTo(new[] { "trim", "upper", "suffix" }));
        });
    }

    [Test]
    public void CascadingChain_PreservesHandlerExecutionOrder()
    {
        var calls = new List<string>();
        var chain = new CascadingChain(
            new RecordingHandler("one", calls, value => value + "-1"),
            new RecordingHandler("two", calls, value => value + "-2"),
            new RecordingHandler("three", calls, value => value + "-3"));

        var result = chain.Handle("start");

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo("start-1-2-3"));
            Assert.That(calls, Is.EqualTo(new[] { "one", "two", "three" }));
        });
    }

    [Test]
    public void EachHandler_IsUnitTestableInIsolation()
    {
        var trim = new TrimHandler();
        var upper = new UpperCaseHandler();
        var suffix = new SuffixHandler("_DONE");

        Assert.Multiple(() =>
        {
            Assert.That(trim.Handle("  request  "), Is.EqualTo("request"));
            Assert.That(upper.Handle("request"), Is.EqualTo("REQUEST"));
            Assert.That(suffix.Handle("REQUEST"), Is.EqualTo("REQUEST_DONE"));
        });
    }

    private sealed class RecordingHandler(string name, IList<string> calls, Func<string, string> transform) : IHandler
    {
        public string Handle(string input)
        {
            calls.Add(name);
            return transform(input);
        }
    }
}