using Structural.Composite.SafeComposite.Api;

namespace Structural.Composite.Tests;

public class SafeCompositeChecklistTests
{
    [Test]
    public void ClientCode_WorksWithComponentInterfaceOnly()
    {
        INode root = BuildSampleSafeTree();

        var totalSize = root.TotalSize();

        Assert.That(totalSize, Is.EqualTo(42));
    }

    [Test]
    public void Leaves_ExecuteDirectly_WithoutChildTraversal()
    {
        INode leaf = new FileNode("report.pdf", 17);

        var totalSize = leaf.TotalSize();

        Assert.That(totalSize, Is.EqualTo(17));
    }

    [Test]
    public void Composite_RecursivelyPropagatesCalls_ToAllChildren()
    {
        var callOrder = new List<string>();

        var root = new FolderNode("root");
        root.Add(new LoggingSafeNode("a", 4, callOrder));
        root.Add(new LoggingSafeNode("b", 5, callOrder));
        root.Add(new LoggingSafeNode("c", 6, callOrder));

        var totalSize = root.TotalSize();

        Assert.Multiple(() =>
        {
            Assert.That(totalSize, Is.EqualTo(15));
            Assert.That(callOrder, Is.EqualTo(new[] { "a", "b", "c" }));
        });
    }

    [Test]
    public void NestedComposites_PreserveCallOrder_AndExpectedOutcome()
    {
        var callOrder = new List<string>();

        var root = new FolderNode("root");
        root.Add(new LoggingSafeNode("root-1", 3, callOrder));

        var nested = new FolderNode("nested");
        nested.Add(new LoggingSafeNode("nested-1", 7, callOrder));
        nested.Add(new LoggingSafeNode("nested-2", 9, callOrder));
        root.Add(nested);

        root.Add(new LoggingSafeNode("root-2", 11, callOrder));

        var totalSize = root.TotalSize();

        Assert.Multiple(() =>
        {
            Assert.That(totalSize, Is.EqualTo(30));
            Assert.That(callOrder, Is.EqualTo(new[] { "root-1", "nested-1", "nested-2", "root-2" }));
        });
    }

    private static INode BuildSampleSafeTree()
    {
        var root = new FolderNode("root");
        root.Add(new FileNode("a.txt", 12));

        var subFolder = new FolderNode("logs");
        subFolder.Add(new FileNode("app.log", 30));
        root.Add(subFolder);

        return root;
    }

    private sealed class LoggingSafeNode : INode
    {
        private readonly int _size;
        private readonly IList<string> _callOrder;

        public LoggingSafeNode(string name, int size, IList<string> callOrder)
        {
            Name = name;
            _size = size;
            _callOrder = callOrder;
        }

        public string Name { get; }

        public int TotalSize()
        {
            _callOrder.Add(Name);
            return _size;
        }
    }
}