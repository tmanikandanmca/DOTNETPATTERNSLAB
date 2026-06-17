using Singleton.BasicSingleton.Api;
using Singleton.ThreadSafeLock.Api;
using Singleton.DoubleCheckedLocking.Api;
using Singleton.LazyInitialization.Api;
using Singleton.EagerInitialization.Api;

namespace Singleton.UnitTests;

public class SingletonDemoSmokeTests
{
    [Test]
    public void Demos_ReturnExpectedPayloadShapes()
    {
        var basic = BasicSingletonDemo.Create();
        var threadSafe = ThreadSafeLockDemo.Create();
        var doubleChecked = DoubleCheckedLockingDemo.Create();
        var lazy = LazyInitializationDemo.Create();
        var eager = EagerInitializationDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(basic, "Pattern"), Is.EqualTo("Singleton"));
            Assert.That(GetProp(basic, "Variant"), Is.EqualTo("Basic Singleton"));
            Assert.That(GetProp(basic, "SameInstance"), Is.EqualTo(true));

            Assert.That(GetProp(threadSafe, "Variant"), Is.EqualTo("Thread-Safe (lock)"));
            Assert.That(GetProp(threadSafe, "SameInstance"), Is.EqualTo(true));

            Assert.That(GetProp(doubleChecked, "Variant"), Is.EqualTo("Double-Checked Locking"));
            Assert.That(GetProp(doubleChecked, "SameInstance"), Is.EqualTo(true));

            Assert.That(GetProp(lazy, "Variant"), Is.EqualTo("Lazy Initialization"));
            Assert.That(GetProp(lazy, "SameInstance"), Is.EqualTo(true));

            Assert.That(GetProp(eager, "Variant"), Is.EqualTo("Eager Initialization"));
            Assert.That(GetProp(eager, "SameInstance"), Is.EqualTo(true));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
