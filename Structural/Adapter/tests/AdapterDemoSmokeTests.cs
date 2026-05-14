using Structural.Adapter.ClassAdapter.Api;
using Structural.Adapter.ObjectAdapter.Api;

namespace Structural.Adapter.Tests;

public class AdapterDemoSmokeTests
{
    [Test]
    public void PatternDemos_ReturnExpectedRootPayloadShapes()
    {
        var classAdapter = Structural.Adapter.ClassAdapter.Api.PatternDemo.Create();
        var objectAdapter = Structural.Adapter.ObjectAdapter.Api.PatternDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(classAdapter, "Pattern"), Is.EqualTo("Adapter"));
            Assert.That(GetProp(classAdapter, "Variant"), Is.EqualTo("Class Adapter"));
            Assert.That(GetProp(classAdapter, "AdaptedOutput"), Is.EqualTo("[APPLICATION] Payment settled"));

            Assert.That(GetProp(objectAdapter, "Pattern"), Is.EqualTo("Adapter"));
            Assert.That(GetProp(objectAdapter, "Variant"), Is.EqualTo("Object Adapter"));
            Assert.That(GetProp(objectAdapter, "AdaptedOutput"), Is.EqualTo("topic=orders.created; payload={ \"id\": 42 }"));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
