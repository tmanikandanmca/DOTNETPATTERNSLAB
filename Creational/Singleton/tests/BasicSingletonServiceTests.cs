using System.Reflection;
using Singleton.BasicSingleton.Api;

namespace Singleton.UnitTests;

public class BasicSingletonServiceTests
{
    [SetUp]
    public void ResetState()
    {
        BasicSingletonService.GetInstance().SetState("Initial");
    }

    [Test]
    public void GetInstance_ReturnsSameReference_ForRepeatedCalls()
    {
        var first = BasicSingletonService.GetInstance();
        var second = BasicSingletonService.GetInstance();

        Assert.That(second, Is.SameAs(first));
    }

    [Test]
    public void Constructor_IsNotPublic()
    {
        var publicConstructors = typeof(BasicSingletonService)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        Assert.That(publicConstructors, Is.Empty);
    }

    [Test]
    public void SharedState_RemainsConsistentAcrossCallers()
    {
        var first = BasicSingletonService.GetInstance();
        first.SetState("Configured");

        var second = BasicSingletonService.GetInstance();

        Assert.Multiple(() =>
        {
            Assert.That(second, Is.SameAs(first));
            Assert.That(second.State, Is.EqualTo("Configured"));
        });
    }
}
