using NUnit.Framework;
using Structural.Decorator.TransparentDecorator.Api;

namespace Decorator.UnitTests;

/// <summary>
/// Validation checklist for Transparent Decorator pattern:
/// ✅ The wrapper still behaves like the component
/// ✅ The original object receives the call when expected
/// ✅ Added behavior runs before or after forwarding in the correct order
/// ✅ Swapping one decorator chain for another does not require client changes
/// </summary>
public class TransparentDecoratorChecklistTests
{
    [Test]
    public void WrapperBehavesLikeComponent_SingleDecorator()
    {
        // Arrange
        IText plain = new PlainText("hello");
        IText decorated = new UppercaseDecorator(plain);

        // Act
        var plainResult = plain.Render();
        var decoratedResult = decorated.Render();

        // Assert
        // Both should be callable through the same interface
        Assert.Multiple(() =>
        {
            Assert.That(plainResult, Is.EqualTo("hello"));
            Assert.That(decoratedResult, Is.EqualTo("HELLO"));
            Assert.That(decorated, Is.InstanceOf<IText>());
        });
    }

    [Test]
    public void OriginalObjectReceivesCall_DecoratorForwardsToInner()
    {
        // Arrange - Create a tracking wrapper to verify forwarding
        var callTracker = new CallTrackingText(new PlainText("tracked"));
        IText decorated = new UppercaseDecorator(callTracker);

        // Act
        var result = decorated.Render();

        // Assert - The inner component was called
        Assert.Multiple(() =>
        {
            Assert.That(callTracker.RenderCallCount, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo("TRACKED"));
        });
    }

    [Test]
    public void AddedBehaviorRunsInCorrectOrder_DecoratorChain()
    {
        // Arrange - Decorator chain: uppercase then bracket
        IText text = new PlainText("order matters");
        text = new UppercaseDecorator(text);
        text = new BracketDecorator(text);

        // Act
        var result = text.Render();

        // Assert - Order is: plain -> uppercase -> bracket
        Assert.That(result, Is.EqualTo("[ORDER MATTERS]"));
    }

    [Test]
    public void AddedBehaviorRunsInCorrectOrder_ReverseOrder()
    {
        // Arrange - Decorator chain: bracket then uppercase (reverse order)
        IText text = new PlainText("order matters");
        text = new BracketDecorator(text);
        text = new UppercaseDecorator(text);

        // Act
        var result = text.Render();

        // Assert - Order is: plain -> bracket -> uppercase
        Assert.That(result, Is.EqualTo("[ORDER MATTERS]"));
    }

    [Test]
    public void SwappingDecoratorChainDoesNotRequireClientChanges()
    {
        // Arrange - A method that works with any IText
        static string ProcessText(IText text) => text.Render();

        var chain1 = new UppercaseDecorator(new PlainText("test"));
        var chain2 = new BracketDecorator(new UppercaseDecorator(new PlainText("test")));

        // Act - The client method doesn't need to change, just the input
        var result1 = ProcessText(chain1);
        var result2 = ProcessText(chain2);

        // Assert - Client code remains the same, chains are interchangeable
        Assert.Multiple(() =>
        {
            Assert.That(result1, Is.EqualTo("TEST"));
            Assert.That(result2, Is.EqualTo("[TEST]"));
        });
    }

    [Test]
    public void DecoratorIsTransparent_ClientCannotDetectDecoration()
    {
        // Arrange
        IText text = new PlainText("hidden");
        IText decorated = new UppercaseDecorator(text);

        // Act & Assert - Both are IText, client can't tell the difference
        Assert.Multiple(() =>
        {
            Assert.That(decorated, Is.InstanceOf<IText>());
            // Can't detect decoration through interface
            Assert.That(text.GetType().Name, Is.EqualTo("PlainText"));
            Assert.That(decorated.GetType().Name, Is.EqualTo("UppercaseDecorator"));
        });
    }

    [Test]
    public void MultipleDecoratorLayers_AllForwardCorrectly()
    {
        // Arrange - Multiple decorators stacked
        var tracker = new CallTrackingText(new PlainText("multi-layer"));
        IText decorated = new UppercaseDecorator(tracker);
        decorated = new BracketDecorator(decorated);

        // Act
        var result = decorated.Render();

        // Assert - Inner call is forwarded through all layers
        Assert.Multiple(() =>
        {
            Assert.That(tracker.RenderCallCount, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo("[MULTI-LAYER]"));
        });
    }

    [Test]
    public void DecorationDoesNotMutateOriginal()
    {
        // Arrange
        var original = new PlainText("immutable");
        IText decorated = new UppercaseDecorator(original);

        // Act
        var originalResult = original.Render();
        var decoratedResult = decorated.Render();

        // Assert - Original is unchanged
        Assert.Multiple(() =>
        {
            Assert.That(originalResult, Is.EqualTo("immutable"));
            Assert.That(decoratedResult, Is.EqualTo("IMMUTABLE"));
        });
    }

    // Helper class to track calls
    private sealed class CallTrackingText : IText
    {
        private readonly IText _inner;
        public int RenderCallCount { get; private set; }

        public CallTrackingText(IText inner) => _inner = inner;

        public string Render()
        {
            RenderCallCount++;
            return _inner.Render();
        }
    }
}
