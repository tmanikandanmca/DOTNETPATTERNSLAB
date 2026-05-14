using AbstractFactory.KitFamilyOfRelatedObjects.Api;
using AbstractFactory.FactoryOfFactories.Api;

namespace AbstractFactory.UnitTests;

public class AbstractFactoryDemoSmokeTests
{
    [Test]
    public void Demos_ReturnExpectedPayloadShapes()
    {
        var kit = KitFamilyOfRelatedObjectsDemo.Create();
        var provider = FactoryOfFactoriesDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(kit, "Pattern"), Is.EqualTo("Abstract Factory"));
            Assert.That(GetProp(kit, "Variant"), Is.EqualTo("Kit (family of related objects)"));
            Assert.That(GetProp(kit, "Family"), Is.EqualTo("Dark"));
            Assert.That(GetProp(kit, "Button"), Is.EqualTo("Dark Button"));
            Assert.That(GetProp(kit, "Card"), Is.EqualTo("Dark Card"));

            Assert.That(GetProp(provider, "Pattern"), Is.EqualTo("Abstract Factory"));
            Assert.That(GetProp(provider, "Variant"), Is.EqualTo("Factory of Factories"));
            Assert.That(GetProp(provider, "Channel"), Is.EqualTo("Email"));
            Assert.That(GetProp(provider, "Example"), Is.Not.Null);
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
