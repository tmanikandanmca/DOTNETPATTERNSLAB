using Structural.Decorator.TransparentDecorator.Api;
using Structural.Decorator.SemiTransparentDecorator.Api;
using Structural.Decorator.DynamicVsStaticDecoration.Api;

namespace Decorator.UnitTests;

public class DecoratorChecklistTests
{
    [Test]
    public void TransparentDecorators_ComposeInExpectedOrder()
    {
        IText text = new PlainText("structural patterns");
        text = new UppercaseDecorator(text);
        text = new BracketDecorator(text);

        Assert.That(text.Render(), Is.EqualTo("[STRUCTURAL PATTERNS]"));
    }

    [Test]
    public void SemiTransparentDecorator_ExposesDecoratorSpecificState_AndRendersTracePrefix()
    {
        var decorator = new TracingDecorator(new SimpleMessage("invoice generated"));
        var rendered = decorator.Render();

        Assert.Multiple(() =>
        {
            Assert.That(decorator.TraceId, Has.Length.EqualTo(32));
            Assert.That(rendered, Does.StartWith($"trace={decorator.TraceId[..8]}::"));
            Assert.That(rendered, Does.EndWith("invoice generated"));
        });
    }

    [Test]
    public void DynamicAndStaticDecoration_ProduceEquivalentShapes()
    {
        var staticDecorated = new StarDecorator(new UpperDecorator(new BaseContent("runtime")));

        IContent dynamicDecorated = new BaseContent("runtime");
        dynamicDecorated = new UpperDecorator(dynamicDecorated);
        dynamicDecorated = new StarDecorator(dynamicDecorated);

        Assert.That(dynamicDecorated.Value(), Is.EqualTo(staticDecorated.Value()));
    }

    [Test]
    public void PatternDemos_ReturnExpectedDecoratorPayloads()
    {
        var transparent = Structural.Decorator.TransparentDecorator.Api.PatternDemo.Create();
        var semi = Structural.Decorator.SemiTransparentDecorator.Api.PatternDemo.Create();
        var dynamic = Structural.Decorator.DynamicVsStaticDecoration.Api.PatternDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(transparent, "Variant"), Is.EqualTo("Transparent Decorator"));
            Assert.That(GetProp(transparent, "Rendered"), Is.EqualTo("[STRUCTURAL PATTERNS]"));

            Assert.That(GetProp(semi, "Variant"), Is.EqualTo("Semi-transparent Decorator"));
            Assert.That(GetProp(semi, "Rendered")?.ToString(), Does.StartWith("trace="));
            Assert.That(GetProp(semi, "DecoratorSpecificState")?.ToString(), Has.Length.EqualTo(32));

            Assert.That(GetProp(dynamic, "Variant"), Is.EqualTo("Dynamic vs Static Decoration"));
            Assert.That(GetProp(dynamic, "StaticDecoration"), Is.EqualTo("*COMPILE TIME*"));
            Assert.That(GetProp(dynamic, "DynamicDecoration"), Is.EqualTo("*RUNTIME*"));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
