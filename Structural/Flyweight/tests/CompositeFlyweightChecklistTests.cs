using Structural.Flyweight.CompositeFlyweight.Api;

namespace Flyweight.UnitTests;

public class CompositeFlyweightChecklistTests
{
    [Test]
    public void FactoryReusesLeafInstances_CompositeContainsSameReferences()
    {
        var factory = new PermissionFactory();

        var read1 = factory.Get("read");
        var write = factory.Get("write");
        var read2 = factory.Get("read");

        var composite = new PermissionComposite(new[] { read1, write, read2 });

        Assert.Multiple(() =>
        {
            Assert.That(read1, Is.SameAs(read2), "Same permission returns same instance");
            Assert.That(factory.SharedLeaves, Is.EqualTo(2), "Factory cached 2 unique leaf permissions");
        });
    }

    [Test]
    public void CompositeApply_CombinesMultipleFlyweights_WithSharedInstances()
    {
        var factory = new PermissionFactory();
        var composite = new PermissionComposite(new[]
        {
            factory.Get("read"),
            factory.Get("write"),
            factory.Get("delete")
        });

        var result = composite.Apply("admin");

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("read"), "Composite output includes read permission");
            Assert.That(result, Does.Contain("write"), "Composite output includes write permission");
            Assert.That(result, Does.Contain("delete"), "Composite output includes delete permission");
            Assert.That(result, Does.Contain("admin"), "Composite output includes user context");
        });
    }

    [Test]
    public void NoLeafDuplication_IdenticalPermissionsShareInstance_InComposite()
    {
        var factory = new PermissionFactory();
        var leaf1 = factory.Get("execute");
        var leaf2 = factory.Get("execute");
        var leaf3 = factory.Get("execute");

        var composite = new PermissionComposite(new[] { leaf1, leaf2, leaf3 });

        Assert.Multiple(() =>
        {
            Assert.That(leaf1, Is.SameAs(leaf2), "Second 'execute' is same instance as first");
            Assert.That(leaf2, Is.SameAs(leaf3), "Third 'execute' is same instance as second");
            Assert.That(factory.SharedLeaves, Is.EqualTo(1), "Only 1 unique leaf despite 3 requests");
        });
    }

    [Test]
    public void ExtrinsicStateNotStored_PassedAtCompositeApplyTime()
    {
        var factory = new PermissionFactory();
        var composite = new PermissionComposite(new[]
        {
            factory.Get("read"),
            factory.Get("write")
        });

        var resultForUser1 = composite.Apply("alice");
        var resultForUser2 = composite.Apply("bob");

        Assert.Multiple(() =>
        {
            Assert.That(resultForUser1, Does.Contain("alice"), "First user name in output");
            Assert.That(resultForUser2, Does.Contain("bob"), "Second user name in output");
            Assert.That(resultForUser1, Is.Not.EqualTo(resultForUser2), "Different extrinsic context produces different results");
        });
    }

    [Test]
    public void ImmutabilityEnforced_LeafPermissionsUnchanged_WhenUsedInComposites()
    {
        var factory = new PermissionFactory();
        var read = factory.Get("read");
        var write = factory.Get("write");

        var originalReadName = read.Permission;
        var originalWriteName = write.Permission;

        var composite = new PermissionComposite(new[] { read, write });
        var result = composite.Apply("user");

        Assert.Multiple(() =>
        {
            Assert.That(read.Permission, Is.EqualTo(originalReadName), "Leaf permission name immutable");
            Assert.That(write.Permission, Is.EqualTo(originalWriteName), "Leaf permission name immutable");
        });
    }

    [Test]
    public void MultipleComposites_SharedLeaves_DontInterference()
    {
        var factory = new PermissionFactory();
        var readLeaf = factory.Get("read");
        var writeLeaf = factory.Get("write");

        var composite1 = new PermissionComposite(new[] { readLeaf, writeLeaf });
        var composite2 = new PermissionComposite(new[] { readLeaf });

        var result1 = composite1.Apply("user1");
        var result2 = composite2.Apply("user2");

        Assert.Multiple(() =>
        {
            Assert.That(result1, Does.Contain("read"), "Composite1 includes read");
            Assert.That(result1, Does.Contain("write"), "Composite1 includes write");
            Assert.That(result2, Does.Contain("read"), "Composite2 includes read");
            Assert.That(result2, Does.Not.Contain("write"), "Composite2 does not include write");
            Assert.That(factory.SharedLeaves, Is.EqualTo(2), "Factory maintains 2 leaves shared across composites");
        });
    }
}
