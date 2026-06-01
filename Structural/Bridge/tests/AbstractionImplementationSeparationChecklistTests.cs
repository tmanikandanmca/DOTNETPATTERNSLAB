using System.Reflection;
using Structural.Bridge.AbstractionImplementationSeparation.Api;

namespace Structural.Bridge.Tests;

public class AbstractionImplementationSeparationChecklistTests
{
    [Test]
    public void Shape_HoldsImplementorViaInterfaceField()
    {
        var rendererField = typeof(Shape).GetField("Renderer", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.That(rendererField, Is.Not.Null);
        Assert.That(rendererField!.FieldType, Is.EqualTo(typeof(IRenderer)));
    }

    [Test]
    public void NewImplementor_CanBeUsedWithoutChangingAbstraction()
    {
        Shape shape = new Circle(new TestRenderer(), 8.5f);

        Assert.That(shape.Draw(), Is.EqualTo("Test circle radius=8.5"));
    }

    [Test]
    public void NewRefinedAbstraction_CanReuseExistingImplementors()
    {
        Shape shape = new Hexagon(new VectorRenderer(), 3.0f);

        Assert.That(shape.Draw(), Is.EqualTo("Vector hexagon radius=3.0"));
    }

    [Test]
    public void CompositionRoot_KnowsBothConcreteSides()
    {
        var demo = PatternDemo.Create();
        var demoType = demo.GetType();

        Assert.Multiple(() =>
        {
            Assert.That(demoType.GetProperty("Pattern")!.GetValue(demo), Is.EqualTo("Bridge"));
            Assert.That(demoType.GetProperty("Variant")!.GetValue(demo), Is.EqualTo("Abstraction-Implementation Separation"));
            Assert.That(demoType.GetProperty("Vector")!.GetValue(demo), Is.EqualTo("Vector circle radius=12.5"));
            Assert.That(demoType.GetProperty("Raster")!.GetValue(demo), Is.EqualTo("Raster circle radius=12.5"));
        });
    }

    private sealed class TestRenderer : IRenderer
    {
        public string RenderCircle(float radius) => $"Test circle radius={radius:0.0}";
    }

    private sealed class Hexagon : Shape
    {
        private readonly float _side;

        public Hexagon(IRenderer renderer, float side) : base(renderer) => _side = side;

        public override string Draw() => Renderer.RenderCircle(_side).Replace("circle", "hexagon", StringComparison.Ordinal);
    }
}