using System.Reflection;
using Singleton.LazyInitialization.Api;

namespace Singleton.UnitTests;

public class LazyInitializationServiceTests
{
    [SetUp]
    public void ResetState()
    {
        LazyInitializationService.GetInstance().SetState("Initial");
    }

    [Test]
    public void GetInstance_ReturnsSameReference_ForRepeatedCalls()
    {
        var first = LazyInitializationService.GetInstance();
        var second = LazyInitializationService.GetInstance();

        Assert.That(second, Is.SameAs(first));
    }

    [Test]
    public void Constructor_IsNotPublic()
    {
        var publicConstructors = typeof(LazyInitializationService)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        Assert.That(publicConstructors, Is.Empty);
    }

    [Test]
    public void SharedState_RemainsConsistentAcrossCallers()
    {
        var first = LazyInitializationService.GetInstance();
        first.SetState("Configured");

        var second = LazyInitializationService.GetInstance();

        Assert.Multiple(() =>
        {
            Assert.That(second, Is.SameAs(first));
            Assert.That(second.State, Is.EqualTo("Configured"));
        });
    }

    [Test]
    public async Task GetInstance_CreatesOnlyOneObject_UnderConcurrentAccess()
    {
        var instances = await Task.WhenAll(
            Enumerable.Range(0, 64).Select(_ => Task.Run(LazyInitializationService.GetInstance)));

        var first = instances[0];

        Assert.Multiple(() =>
        {
            Assert.That(instances.All(instance => ReferenceEquals(instance, first)), Is.True);
            Assert.That(instances.Select(instance => instance.Id).Distinct().Count(), Is.EqualTo(1));
        });
    }
}
