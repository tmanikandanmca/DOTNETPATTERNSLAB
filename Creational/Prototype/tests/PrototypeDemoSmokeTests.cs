using Prototype.ShallowCopy.Api;
using Prototype.DeepCopy.Api;
using Prototype.CloneRegistry.Api;

namespace Prototype.UnitTests;

public class PrototypeDemoSmokeTests
{
    [Test]
    public void Demos_ReturnExpectedPayloadShapes()
    {
        var shallow = ShallowCopyDemo.Create();
        var deep = DeepCopyDemo.Create();
        var registry = CloneRegistryDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(shallow, "Pattern"), Is.EqualTo("Prototype"));
            Assert.That(GetProp(shallow, "Variant"), Is.EqualTo("Shallow Copy"));
            Assert.That(GetProp(shallow, "SameNestedReference"), Is.EqualTo(true));

            Assert.That(GetProp(deep, "Pattern"), Is.EqualTo("Prototype"));
            Assert.That(GetProp(deep, "Variant"), Is.EqualTo("Deep Copy"));
            Assert.That(GetProp(deep, "SameNestedReference"), Is.EqualTo(false));

            Assert.That(GetProp(registry, "Pattern"), Is.EqualTo("Prototype"));
            Assert.That(GetProp(registry, "Variant"), Is.EqualTo("Clone Registry"));
            Assert.That(GetProp(registry, "Title"), Is.EqualTo("Invoice for April"));
            Assert.That(GetProp(registry, "Category"), Is.EqualTo("Billing"));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
