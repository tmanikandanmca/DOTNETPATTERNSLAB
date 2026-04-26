using System.Reflection;
using Singleton.EagerInitialization.Api;

namespace Singleton.UnitTests;

public class EagerInitializationServiceTests
{
    [SetUp]
    public void ResetState()
    {
        EagerInitializationService.GetInstance().SetState("Initial");
    }

    [Test]
    public void GetInstance_ReturnsSameReference_ForRepeatedCalls()
    {
        var first = EagerInitializationService.GetInstance();
        var second = EagerInitializationService.GetInstance();

        Assert.That(second, Is.SameAs(first));
    }

    [Test]
    public void Constructor_IsNotPublic()
    {
        var publicConstructors = typeof(EagerInitializationService)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        Assert.That(publicConstructors, Is.Empty);
    }

    [Test]
    public void SharedState_RemainsConsistentAcrossCallers()
    {
        var first = EagerInitializationService.GetInstance();
        first.SetState("Configured");

        var second = EagerInitializationService.GetInstance();

        Assert.Multiple(() =>
        {
            Assert.That(second, Is.SameAs(first));
            Assert.That(second.State, Is.EqualTo("Configured"));
        });
    }
}
