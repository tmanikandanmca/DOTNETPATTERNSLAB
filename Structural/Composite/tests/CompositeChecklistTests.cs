using Structural.Composite.SafeComposite.Api;
using Structural.Composite.TransparentComposite.Api;

namespace Composite.UnitTests;

public class CompositeChecklistTests
{
    [Test]
    public void SafeComposite_AggregatesRecursivelyAcrossNestedFolders()
    {
        var root = new FolderNode("root");
        var nested = new FolderNode("nested");
        nested.Add(new FileNode("c.txt", 8));

        root.Add(new FileNode("a.txt", 12));
        root.Add(new FileNode("b.log", 30));
        root.Add(nested);

        Assert.That(root.TotalSize(), Is.EqualTo(50));
    }

    [Test]
    public void TransparentComposite_LeafRejectsChildMutations_WithClearError()
    {
        IOrganizationUnit leaf = new EmployeeLeaf("Isha");

        var addEx = Assert.Throws<NotSupportedException>(() => leaf.Add(new EmployeeLeaf("Another")));
        var removeEx = Assert.Throws<NotSupportedException>(() => leaf.Remove(new EmployeeLeaf("Another")));

        Assert.Multiple(() =>
        {
            Assert.That(addEx!.Message, Is.EqualTo("Leaf cannot add children."));
            Assert.That(removeEx!.Message, Is.EqualTo("Leaf cannot remove children."));
        });
    }

    [Test]
    public void TransparentComposite_AddAndRemove_UpdatesHeadcount()
    {
        var team = new TeamComposite("Payments");
        var one = new EmployeeLeaf("Nila");
        var two = new EmployeeLeaf("Arun");

        team.Add(one);
        team.Add(two);
        team.Remove(one);

        Assert.That(team.Headcount(), Is.EqualTo(1));
    }

    [Test]
    public void PatternDemos_ReturnExpectedCompositePayloads()
    {
        var safe = Structural.Composite.SafeComposite.Api.PatternDemo.Create();
        var transparent = Structural.Composite.TransparentComposite.Api.PatternDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(safe, "Pattern"), Is.EqualTo("Composite"));
            Assert.That(GetProp(safe, "Variant"), Is.EqualTo("Safe Composite"));
            Assert.That(GetProp(safe, "Size"), Is.EqualTo(42));

            Assert.That(GetProp(transparent, "Pattern"), Is.EqualTo("Composite"));
            Assert.That(GetProp(transparent, "Variant"), Is.EqualTo("Transparent Composite"));
            Assert.That(GetProp(transparent, "Headcount"), Is.EqualTo(2));
            Assert.That(GetProp(transparent, "LeafOperationBehavior"), Does.Contain("Leaf cannot add children"));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
