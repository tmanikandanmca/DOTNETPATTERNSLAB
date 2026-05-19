using NUnit.Framework;
using Structural.Decorator.DynamicVsStaticDecoration.Api;

namespace Decorator.UnitTests;

/// <summary>
/// Validation checklist for Dynamic vs Static Decoration pattern:
/// ✅ The wrapper still behaves like the component
/// ✅ The original object receives the call when expected
/// ✅ Added behavior runs before or after forwarding in the correct order
/// ✅ Swapping one decorator chain for another does not require client changes
/// Additional: ✅ Static and dynamic compositions produce equivalent results
/// ✅ Decorator order is preserved in both static and dynamic approaches
/// </summary>
public class DynamicVsStaticDecorationChecklistTests
{
    [Test]
    public void WrapperBehavesLikeComponent_StaticDecoration()
    {
        // Arrange
        IContent plain = new BaseContent("hello");
        IContent decorated = new UpperDecorator(plain);

        // Act
        var plainResult = plain.Value();
        var decoratedResult = decorated.Value();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(plainResult, Is.EqualTo("hello"));
            Assert.That(decoratedResult, Is.EqualTo("HELLO"));
            Assert.That(decorated, Is.InstanceOf<IContent>());
        });
    }

    [Test]
    public void WrapperBehavesLikeComponent_DynamicDecoration()
    {
        // Arrange
        IContent content = new BaseContent("hello");
        var decorators = new Func<IContent, IContent>[] { c => new UpperDecorator(c) };

        // Act
        foreach (var decorate in decorators)
        {
            content = decorate(content);
        }
        var result = content.Value();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo("HELLO"));
            Assert.That(content, Is.InstanceOf<IContent>());
        });
    }

    [Test]
    public void OriginalObjectReceivesCall_StaticDecorationChain()
    {
        // Arrange
        var tracker = new CallTrackingContent(new BaseContent("forwarded"));
        IContent decorated = new UpperDecorator(tracker);

        // Act
        var result = decorated.Value();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(tracker.ValueCallCount, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo("FORWARDED"));
        });
    }

    [Test]
    public void OriginalObjectReceivesCall_DynamicDecorationChain()
    {
        // Arrange
        var tracker = new CallTrackingContent(new BaseContent("forwarded"));
        IContent content = tracker;

        var decorators = new Func<IContent, IContent>[] { c => new UpperDecorator(c) };

        // Act
        foreach (var decorate in decorators)
        {
            content = decorate(content);
        }
        var result = content.Value();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(tracker.ValueCallCount, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo("FORWARDED"));
        });
    }

    [Test]
    public void AddedBehaviorRunsInCorrectOrder_StaticDecoration()
    {
        // Arrange - Static: upper then star
        IContent content = new StarDecorator(new UpperDecorator(new BaseContent("order")));

        // Act
        var result = content.Value();

        // Assert
        Assert.That(result, Is.EqualTo("*ORDER*"));
    }

    [Test]
    public void AddedBehaviorRunsInCorrectOrder_DynamicDecoration()
    {
        // Arrange - Dynamic: same order (upper then star)
        IContent content = new BaseContent("order");
        var decorators = new Func<IContent, IContent>[]
        {
            c => new UpperDecorator(c),
            c => new StarDecorator(c)
        };

        // Act
        foreach (var decorate in decorators)
        {
            content = decorate(content);
        }
        var result = content.Value();

        // Assert
        Assert.That(result, Is.EqualTo("*ORDER*"));
    }

    [Test]
    public void StaticAndDynamicProduceEquivalentResults()
    {
        // Arrange
        var staticResult = new StarDecorator(new UpperDecorator(new BaseContent("test"))).Value();

        IContent dynamic = new BaseContent("test");
        var decorators = new Func<IContent, IContent>[]
        {
            c => new UpperDecorator(c),
            c => new StarDecorator(c)
        };
        foreach (var decorate in decorators)
        {
            dynamic = decorate(dynamic);
        }
        var dynamicResult = dynamic.Value();

        // Act & Assert
        Assert.That(staticResult, Is.EqualTo(dynamicResult));
        Assert.That(staticResult, Is.EqualTo("*TEST*"));
    }

    [Test]
    public void SwappingDecoratorChainDoesNotRequireClientChanges_ClientWorksWithInterface()
    {
        // Arrange - A method that works with any IContent
        static string ProcessContent(IContent content) => content.Value();

        var chain1 = new UpperDecorator(new BaseContent("test"));
        var chain2 = new StarDecorator(new UpperDecorator(new BaseContent("test")));

        // Act
        var result1 = ProcessContent(chain1);
        var result2 = ProcessContent(chain2);

        // Assert - Client doesn't change, chains are interchangeable
        Assert.Multiple(() =>
        {
            Assert.That(result1, Is.EqualTo("TEST"));
            Assert.That(result2, Is.EqualTo("*TEST*"));
        });
    }

    [Test]
    public void DynamicDecoration_ReverseOrderProducesExpectedResult()
    {
        // Arrange - Dynamic with reverse order (star then upper)
        IContent content = new BaseContent("reverse");
        var decorators = new Func<IContent, IContent>[]
        {
            c => new StarDecorator(c),
            c => new UpperDecorator(c)
        };

        // Act
        foreach (var decorate in decorators)
        {
            content = decorate(content);
        }
        var result = content.Value();

        // Assert - Order matters!
        Assert.That(result, Is.EqualTo("*REVERSE*"));
    }

    [Test]
    public void StaticDecoration_ReverseOrderProducesExpectedResult()
    {
        // Arrange - Static: star then upper (reverse)
        IContent content = new UpperDecorator(new StarDecorator(new BaseContent("reverse")));

        // Act
        var result = content.Value();

        // Assert
        Assert.That(result, Is.EqualTo("*REVERSE*"));
    }

    [Test]
    public void DynamicDecoration_VariableNumberOfDecorators()
    {
        // Arrange - Apply different numbers of decorators dynamically
        IContent content1 = new BaseContent("abc");
        IContent content2 = new BaseContent("abc");
        IContent content3 = new BaseContent("abc");

        var decorators = new Func<IContent, IContent>[] { c => new UpperDecorator(c), c => new StarDecorator(c) };

        // Act - Apply 0, 1, 2 decorators
        // content1: no decorators
        var result1 = content1.Value();

        // content2: 1 decorator
        content2 = decorators[0](content2);
        var result2 = content2.Value();

        // content3: 2 decorators
        for (int i = 0; i < decorators.Length; i++)
        {
            content3 = decorators[i](content3);
        }
        var result3 = content3.Value();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result1, Is.EqualTo("abc"));
            Assert.That(result2, Is.EqualTo("ABC"));
            Assert.That(result3, Is.EqualTo("*ABC*"));
        });
    }

    [Test]
    public void StaticDecoration_CompileTimeKnown()
    {
        // Arrange - Static composition is known at compile time
        IContent content = new StarDecorator(new UpperDecorator(new BaseContent("compile")));

        // Act
        var result = content.Value();
        var type = content.GetType().Name;

        // Assert - Type is known statically
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo("*COMPILE*"));
            Assert.That(type, Is.EqualTo("StarDecorator"));
        });
    }

    [Test]
    public void DynamicDecoration_CanBeDeterminedByCondition()
    {
        // Arrange
        IContent content = new BaseContent("conditional");
        bool shouldApplyUpper = true;
        bool shouldApplyStar = false;

        // Act
        if (shouldApplyUpper)
            content = new UpperDecorator(content);
        if (shouldApplyStar)
            content = new StarDecorator(content);

        var result = content.Value();

        // Assert
        Assert.That(result, Is.EqualTo("CONDITIONAL"));
    }

    [Test]
    public void DecorationDoesNotMutateOriginal_StaticDecoration()
    {
        // Arrange
        var original = new BaseContent("original");
        IContent decorated = new UpperDecorator(original);

        // Act
        var originalResult = original.Value();
        var decoratedResult = decorated.Value();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(originalResult, Is.EqualTo("original"));
            Assert.That(decoratedResult, Is.EqualTo("ORIGINAL"));
        });
    }

    [Test]
    public void DecorationDoesNotMutateOriginal_DynamicDecoration()
    {
        // Arrange
        var original = new BaseContent("original");
        IContent content = original;
        content = new UpperDecorator(content);

        // Act
        var originalResult = original.Value();
        var decoratedResult = content.Value();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(originalResult, Is.EqualTo("original"));
            Assert.That(decoratedResult, Is.EqualTo("ORIGINAL"));
        });
    }

    // Helper class to track calls
    private sealed class CallTrackingContent : IContent
    {
        private readonly IContent _inner;
        public int ValueCallCount { get; private set; }

        public CallTrackingContent(IContent inner) => _inner = inner;

        public string Value()
        {
            ValueCallCount++;
            return _inner.Value();
        }
    }
}
