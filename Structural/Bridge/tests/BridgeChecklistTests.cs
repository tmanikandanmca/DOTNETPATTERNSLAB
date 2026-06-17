using Structural.Bridge.AbstractionImplementationSeparation.Api;
using Structural.Bridge.RefinedAbstractionVsConcreteImplementor.Api;

namespace Bridge.UnitTests;

public class BridgeChecklistTests
{
    [Test]
    public void Circle_DelegatesRendering_ToSelectedImplementor()
    {
        var vector = new Circle(new VectorRenderer(), 12.5f);
        var raster = new Circle(new RasterRenderer(), 12.5f);

        Assert.Multiple(() =>
        {
            Assert.That(vector.Draw(), Is.EqualTo("Vector circle radius=12.5"));
            Assert.That(raster.Draw(), Is.EqualTo("Raster circle radius=12.5"));
        });
    }

    [Test]
    public void RefinedAbstractions_AlterBehavior_WithoutChangingChannel()
    {
        Notification standardEmail = new StandardNotification(new EmailChannel());
        Notification prioritySms = new PriorityNotification(new SmsChannel());

        Assert.Multiple(() =>
        {
            Assert.That(standardEmail.Dispatch("Build succeeded"), Is.EqualTo("EMAIL::Build succeeded"));
            Assert.That(prioritySms.Dispatch("API down"), Is.EqualTo("SMS::[PRIORITY] API down"));
        });
    }

    [Test]
    public void PatternDemos_ReturnExpectedBridgeVariants()
    {
        var first = Structural.Bridge.AbstractionImplementationSeparation.Api.PatternDemo.Create();
        var second = Structural.Bridge.RefinedAbstractionVsConcreteImplementor.Api.PatternDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(first, "Pattern"), Is.EqualTo("Bridge"));
            Assert.That(GetProp(first, "Variant"), Is.EqualTo("Abstraction-Implementation Separation"));
            Assert.That(GetProp(first, "Vector"), Is.EqualTo("Vector circle radius=12.5"));
            Assert.That(GetProp(first, "Raster"), Is.EqualTo("Raster circle radius=12.5"));

            Assert.That(GetProp(second, "Pattern"), Is.EqualTo("Bridge"));
            Assert.That(GetProp(second, "Variant"), Is.EqualTo("Refined Abstraction vs Concrete Implementor"));
            Assert.That(GetProp(second, "Standard"), Is.EqualTo("EMAIL::Build succeeded"));
            Assert.That(GetProp(second, "Priority"), Is.EqualTo("SMS::[PRIORITY] API down"));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
