using System.Reflection;
using Singleton.ThreadSafeLock.Api;

namespace Singleton.UnitTests;

public class ThreadSafeLockServiceTests
{
    [SetUp]
    public void ResetState()
    {
        ThreadSafeLockService.GetInstance().SetState("Initial");
    }

    [Test]
    public void GetInstance_ReturnsSameReference_ForRepeatedCalls()
    {
        var first = ThreadSafeLockService.GetInstance();
        var second = ThreadSafeLockService.GetInstance();

        Assert.That(second, Is.SameAs(first));
    }

    [Test]
    public void Constructor_IsNotPublic()
    {
        var publicConstructors = typeof(ThreadSafeLockService)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        Assert.That(publicConstructors, Is.Empty);
    }

    [Test]
    public void SharedState_RemainsConsistentAcrossCallers()
    {
        var first = ThreadSafeLockService.GetInstance();
        first.SetState("Configured");

        var second = ThreadSafeLockService.GetInstance();

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
            Enumerable.Range(0, 64).Select(_ => Task.Run(ThreadSafeLockService.GetInstance)));

        var first = instances[0];

        Assert.Multiple(() =>
        {
            Assert.That(instances.All(instance => ReferenceEquals(instance, first)), Is.True);
            Assert.That(instances.Select(instance => instance.Id).Distinct().Count(), Is.EqualTo(1));
        });
    }
}
