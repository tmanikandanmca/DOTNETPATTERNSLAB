using System.Reflection;
using Singleton.DoubleCheckedLocking.Api;

namespace Singleton.UnitTests;

public class DoubleCheckedLockingServiceTests
{
    [SetUp]
    public void ResetState()
    {
        DoubleCheckedLockingService.GetInstance().SetState("Initial");
    }

    [Test]
    public void GetInstance_ReturnsSameReference_ForRepeatedCalls()
    {
        var first = DoubleCheckedLockingService.GetInstance();
        var second = DoubleCheckedLockingService.GetInstance();

        Assert.That(second, Is.SameAs(first));
    }

    [Test]
    public void Constructor_IsNotPublic()
    {
        var publicConstructors = typeof(DoubleCheckedLockingService)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        Assert.That(publicConstructors, Is.Empty);
    }

    [Test]
    public void SharedState_RemainsConsistentAcrossCallers()
    {
        var first = DoubleCheckedLockingService.GetInstance();
        first.SetState("Configured");

        var second = DoubleCheckedLockingService.GetInstance();

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
            Enumerable.Range(0, 64).Select(_ => Task.Run(DoubleCheckedLockingService.GetInstance)));

        var first = instances[0];

        Assert.Multiple(() =>
        {
            Assert.That(instances.All(instance => ReferenceEquals(instance, first)), Is.True);
            Assert.That(instances.Select(instance => instance.Id).Distinct().Count(), Is.EqualTo(1));
        });
    }
}
