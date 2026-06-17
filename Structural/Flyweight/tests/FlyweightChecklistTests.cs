using Structural.Flyweight.IntrinsicVsExtrinsicState.Api;
using Structural.Flyweight.CompositeFlyweight.Api;

namespace Flyweight.UnitTests;

public class FlyweightChecklistTests
{
    [Test]
    public void IntrinsicStateFactory_ReusesFlyweights_ForSameKey()
    {
        var factory = new GlyphFactory();

        var first = factory.Get('M', "Consolas");
        var second = factory.Get('M', "Consolas");
        var third = factory.Get('M', "Arial");

        Assert.Multiple(() =>
        {
            Assert.That(second, Is.SameAs(first));
            Assert.That(third, Is.Not.SameAs(first));
            Assert.That(factory.SharedInstances, Is.EqualTo(2));
        });
    }

    [Test]
    public void Glyph_UsesExtrinsicState_WhenDrawing()
    {
        var glyph = new GlyphFactory().Get('O', "Consolas");
        var draw = glyph.Draw(10, 5, "blue");

        Assert.That(draw, Is.EqualTo("O@(10,5) font=Consolas color=blue"));
    }

    [Test]
    public void CompositeFlyweight_AppliesAllPermissions_AndReusesLeavesCaseInsensitively()
    {
        var factory = new PermissionFactory();
        var composite = new PermissionComposite(new IPermissionFlyweight[]
        {
            factory.Get("read"),
            factory.Get("WRITE"),
            factory.Get("READ")
        });

        var applied = composite.Apply("alex");

        Assert.Multiple(() =>
        {
            Assert.That(applied, Is.EqualTo("alex:read, alex:WRITE, alex:read"));
            Assert.That(factory.SharedLeaves, Is.EqualTo(2));
        });
    }

    [Test]
    public void PatternDemos_ReturnExpectedFlyweightPayloads()
    {
        var intrinsic = Structural.Flyweight.IntrinsicVsExtrinsicState.Api.PatternDemo.Create();
        var composite = Structural.Flyweight.CompositeFlyweight.Api.PatternDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(intrinsic, "Variant"), Is.EqualTo("Intrinsic vs Extrinsic State"));
            Assert.That(GetProp(intrinsic, "SharedFlyweights"), Is.EqualTo(2));

            Assert.That(GetProp(composite, "Variant"), Is.EqualTo("Composite Flyweight"));
            Assert.That(GetProp(composite, "Applied"), Is.EqualTo("alex:read, alex:write, alex:read"));
            Assert.That(GetProp(composite, "SharedLeafCount"), Is.EqualTo(2));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
