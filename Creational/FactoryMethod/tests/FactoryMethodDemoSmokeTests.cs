using FactoryMethod.SimpleFactory.Api;
using FactoryMethod.ParameterizedFactory.Api;
using FactoryMethod.VirtualConstructor.Api;

namespace FactoryMethod.UnitTests;

public class FactoryMethodDemoSmokeTests
{
    [Test]
    public void Demos_ReturnExpectedPayloadShapes()
    {
        var simple = SimpleFactoryDemo.Create();
        var parameterized = ParameterizedFactoryDemo.Create();
        var virtualConstructor = VirtualConstructorDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(simple, "Pattern"), Is.EqualTo("Factory Method"));
            Assert.That(GetProp(simple, "Variant"), Is.EqualTo("Simple Factory (static)"));
            Assert.That(GetProp(simple, "Product"), Is.EqualTo("PDF"));

            Assert.That(GetProp(parameterized, "Pattern"), Is.EqualTo("Factory Method"));
            Assert.That(GetProp(parameterized, "Variant"), Is.EqualTo("Parameterized Factory"));
            Assert.That(GetProp(parameterized, "Product"), Is.EqualTo("SMS"));

            Assert.That(GetProp(virtualConstructor, "Pattern"), Is.EqualTo("Factory Method"));
            Assert.That(GetProp(virtualConstructor, "Variant"), Is.EqualTo("Virtual Constructor"));
            Assert.That(GetProp(virtualConstructor, "Output"), Does.StartWith("{ \"message\": "));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
