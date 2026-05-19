using NUnit.Framework;
using Structural.Decorator.SemiTransparentDecorator.Api;

namespace Decorator.UnitTests;

/// <summary>
/// Validation checklist for Semi-Transparent Decorator pattern:
/// ✅ The wrapper still behaves like the component
/// ✅ The original object receives the call when expected
/// ✅ Added behavior runs before or after forwarding in the correct order
/// ✅ Swapping one decorator chain for another does not require client changes
/// Additional: ✅ Decorator-specific features are accessible when needed
/// </summary>
public class SemiTransparentDecoratorChecklistTests
{
    [Test]
    public void WrapperBehavesLikeComponent_ThroughInterface()
    {
        // Arrange
        IMessage plain = new SimpleMessage("hello");
        IMessage decorated = new TracingDecorator(plain);

        // Act
        var plainResult = plain.Render();
        var decoratedResult = decorated.Render();

        // Assert - Both implement IMessage
        Assert.Multiple(() =>
        {
            Assert.That(plainResult, Is.EqualTo("hello"));
            Assert.That(decoratedResult, Does.Contain("hello"));
            Assert.That(decorated, Is.InstanceOf<IMessage>());
        });
    }

    [Test]
    public void OriginalObjectReceivesCall_DecoratorForwardsToInner()
    {
        // Arrange - Create tracking wrapper
        var tracker = new CallTrackingMessage(new SimpleMessage("forwarded"));
        IMessage decorated = new TracingDecorator(tracker);

        // Act
        var result = decorated.Render();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(tracker.RenderCallCount, Is.EqualTo(1));
            Assert.That(result, Does.Contain("forwarded"));
        });
    }

    [Test]
    public void AddedBehaviorRunsBeforeForwarding_TracingPrefixes()
    {
        // Arrange
        var inner = new SimpleMessage("message");
        var decorated = new TracingDecorator(inner);

        // Act
        var result = decorated.Render();

        // Assert - Trace ID added before the inner message
        Assert.Multiple(() =>
        {
            Assert.That(result, Does.StartWith("trace="));
            Assert.That(result, Does.Contain("::"));
            Assert.That(result, Does.Contain("message"));
        });
    }

    [Test]
    public void DecoratorSpecificFeatureIsAccessible_CastingToDecorator()
    {
        // Arrange
        IMessage message = new TracingDecorator(new SimpleMessage("test"));

        // Act - Access decorator-specific feature
        var traceId = (message as TracingDecorator)?.TraceId;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(traceId, Is.Not.Null);
            Assert.That(traceId, Has.Length.EqualTo(32));  // GUID format
        });
    }

    [Test]
    public void DecoratorSpecificFeatureNotAccessibleViaInterface()
    {
        // Arrange - Using only interface, not concrete type
        IMessage message = new TracingDecorator(new SimpleMessage("test"));

        // Act & Assert - TraceId is not part of the interface
        Assert.That(message, Is.InstanceOf<IMessage>());
        // The interface doesn't expose TraceId
        var hasTraceIdProperty = typeof(IMessage).GetProperty("TraceId");
        Assert.That(hasTraceIdProperty, Is.Null);
    }

    [Test]
    public void SwappingDecoratorChainDoesNotRequireClientChanges_InterfacePolymorphism()
    {
        // Arrange - A generic processor that only knows IMessage
        static string ProcessMessage(IMessage msg) => msg.Render();

        var simple = new SimpleMessage("simple");
        var traced = new TracingDecorator(new SimpleMessage("traced"));

        // Act - Client doesn't need to change
        var result1 = ProcessMessage(simple);
        var result2 = ProcessMessage(traced);

        // Assert - Both work with the same client method
        Assert.Multiple(() =>
        {
            Assert.That(result1, Is.EqualTo("simple"));
            Assert.That(result2, Does.Contain("traced"));
        });
    }

    [Test]
    public void MultipleTracingDecorators_EachHasUniqueTraceId()
    {
        // Arrange
        var decorated1 = new TracingDecorator(new SimpleMessage("msg1"));
        var decorated2 = new TracingDecorator(new SimpleMessage("msg2"));

        // Act
        var id1 = decorated1.TraceId;
        var id2 = decorated2.TraceId;

        // Assert - Each decorator instance has its own trace ID
        Assert.Multiple(() =>
        {
            Assert.That(id1, Is.Not.EqualTo(id2));
            Assert.That(id1, Has.Length.EqualTo(32));
            Assert.That(id2, Has.Length.EqualTo(32));
        });
    }

    [Test]
    public void DecoratorIsNotInvisibleToInspection_TypeCheckingPossible()
    {
        // Arrange
        IMessage message = new TracingDecorator(new SimpleMessage("inspection"));

        // Act - Client can detect what type it is
        var isTraced = message is TracingDecorator;
        var concreteType = message.GetType().Name;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isTraced, Is.True);
            Assert.That(concreteType, Is.EqualTo("TracingDecorator"));
        });
    }

    [Test]
    public void GenericClientWorksWithAnyImplementation_NotAffectedBySemiTransparence()
    {
        // Arrange - A method that uses only the interface
        IMessage ProcessGeneric(IMessage msg)
        {
            var result = msg.Render();
            return new SimpleMessage(result);
        }

        var plain = new SimpleMessage("generic");
        var traced = new TracingDecorator(new SimpleMessage("traced"));

        // Act
        var result1 = ProcessGeneric(plain);
        var result2 = ProcessGeneric(traced);

        // Assert - Generic method works with both
        Assert.Multiple(() =>
        {
            Assert.That(result1, Is.InstanceOf<IMessage>());
            Assert.That(result2, Is.InstanceOf<IMessage>());
        });
    }

    [Test]
    public void NestedDecorators_InnerDecoratorSpecificFeaturesNotDirectlyAccessible()
    {
        // Arrange - Nested: Tracing(Tracing(SimpleMessage))
        IMessage inner = new TracingDecorator(new SimpleMessage("inner"));
        IMessage outer = new TracingDecorator(inner);

        // Act
        var outerTraceId = (outer as TracingDecorator)?.TraceId;
        var innerTraceId = (inner as TracingDecorator)?.TraceId;

        // Assert - Can access outer, but inner requires unwrapping
        Assert.Multiple(() =>
        {
            Assert.That(outerTraceId, Is.Not.Null);
            Assert.That(innerTraceId, Is.Not.Null);
            Assert.That(outerTraceId, Is.Not.EqualTo(innerTraceId));
        });
    }

    // Helper class to track calls
    private sealed class CallTrackingMessage : IMessage
    {
        private readonly IMessage _inner;
        public int RenderCallCount { get; private set; }

        public CallTrackingMessage(IMessage inner) => _inner = inner;

        public string Render()
        {
            RenderCallCount++;
            return _inner.Render();
        }
    }
}
