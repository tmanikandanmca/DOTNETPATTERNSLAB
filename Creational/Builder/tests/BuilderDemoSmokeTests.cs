using Builder.FluentBuilder.Api;
using Builder.StepBuilder.Api;
using Builder.DirectorBasedBuilder.Api;

namespace Builder.UnitTests;

public class BuilderDemoSmokeTests
{
    [Test]
    public void Demos_ReturnExpectedPayloadShapes()
    {
        var fluent = FluentBuilderDemo.Create();
        var step = StepBuilderDemo.Create();
        var director = DirectorBasedBuilderDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(fluent, "Pattern"), Is.EqualTo("Builder"));
            Assert.That(GetProp(fluent, "Variant"), Is.EqualTo("Fluent Builder"));
            Assert.That(GetProp(fluent, "Endpoint"), Is.EqualTo("/orders"));

            Assert.That(GetProp(step, "Pattern"), Is.EqualTo("Builder"));
            Assert.That(GetProp(step, "Variant"), Is.EqualTo("Step Builder"));
            Assert.That(GetProp(step, "Environment"), Is.EqualTo("Production"));

            Assert.That(GetProp(director, "Pattern"), Is.EqualTo("Builder"));
            Assert.That(GetProp(director, "Variant"), Is.EqualTo("Director-based Builder"));
            Assert.That(GetProp(director, "Bread"), Is.EqualTo("Whole Grain"));
            Assert.That(GetProp(director, "Main"), Is.EqualTo("Chicken"));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
