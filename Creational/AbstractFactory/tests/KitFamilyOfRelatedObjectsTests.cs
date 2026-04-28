using AbstractFactory.KitFamilyOfRelatedObjects.Api;

namespace AbstractFactory.UnitTests;

public class KitFamilyOfRelatedObjectsTests
{
    [Test]
    public void ConcreteFactory_ReturnsMatchingProductFamilyMembers()
    {
        IUiKitFactory dark = new DarkUiKitFactory();
        IUiKitFactory light = new LightUiKitFactory();

        Assert.Multiple(() =>
        {
            Assert.That(dark.CreateButton().Label(), Is.EqualTo("Dark Button"));
            Assert.That(dark.CreateCard().Style(), Is.EqualTo("Dark Card"));
            Assert.That(light.CreateButton().Label(), Is.EqualTo("Light Button"));
            Assert.That(light.CreateCard().Style(), Is.EqualTo("Light Card"));
        });
    }
}
