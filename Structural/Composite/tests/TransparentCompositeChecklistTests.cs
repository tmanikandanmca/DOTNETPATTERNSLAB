using Structural.Composite.TransparentComposite.Api;

namespace Structural.Composite.Tests;

public class TransparentCompositeChecklistTests
{
    [Test]
    public void ClientCode_WorksWithComponentInterfaceOnly()
    {
        IOrganizationUnit unit = BuildSampleTransparentTree();

        var totalHeadcount = unit.Headcount();

        Assert.That(totalHeadcount, Is.EqualTo(4));
    }

    [Test]
    public void Leaves_ExecuteDirectly_WithoutChildTraversal()
    {
        IOrganizationUnit leaf = new EmployeeLeaf("Anika");

        var result = leaf.Headcount();

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Composite_RecursivelyPropagatesCalls_ToAllChildren()
    {
        var callOrder = new List<string>();

        var team = new TeamComposite("Payments");
        team.Add(new LoggingTransparentUnit("u1", 1, callOrder));
        team.Add(new LoggingTransparentUnit("u2", 2, callOrder));
        team.Add(new LoggingTransparentUnit("u3", 3, callOrder));

        var totalHeadcount = team.Headcount();

        Assert.Multiple(() =>
        {
            Assert.That(totalHeadcount, Is.EqualTo(6));
            Assert.That(callOrder, Is.EqualTo(new[] { "u1", "u2", "u3" }));
        });
    }

    [Test]
    public void NestedComposites_PreserveCallOrder_AndExpectedOutcome()
    {
        var callOrder = new List<string>();

        var root = new TeamComposite("Engineering");
        root.Add(new LoggingTransparentUnit("root-1", 1, callOrder));

        var nested = new TeamComposite("Platform");
        nested.Add(new LoggingTransparentUnit("nested-1", 2, callOrder));
        nested.Add(new LoggingTransparentUnit("nested-2", 3, callOrder));

        root.Add(nested);
        root.Add(new LoggingTransparentUnit("root-2", 4, callOrder));

        var totalHeadcount = root.Headcount();

        Assert.Multiple(() =>
        {
            Assert.That(totalHeadcount, Is.EqualTo(10));
            Assert.That(callOrder, Is.EqualTo(new[] { "root-1", "nested-1", "nested-2", "root-2" }));
        });
    }

    [Test]
    public void LeafChildManagementOperations_ThrowClearErrors()
    {
        IOrganizationUnit leaf = new EmployeeLeaf("Isha");

        var addError = Assert.Throws<NotSupportedException>(() => leaf.Add(new EmployeeLeaf("ShouldFail")));
        var removeError = Assert.Throws<NotSupportedException>(() => leaf.Remove(new EmployeeLeaf("ShouldFail")));

        Assert.Multiple(() =>
        {
            Assert.That(addError!.Message, Is.EqualTo("Leaf cannot add children."));
            Assert.That(removeError!.Message, Is.EqualTo("Leaf cannot remove children."));
        });
    }

    private static IOrganizationUnit BuildSampleTransparentTree()
    {
        var root = new TeamComposite("Payments");
        root.Add(new EmployeeLeaf("A"));

        var nested = new TeamComposite("Risk");
        nested.Add(new EmployeeLeaf("B"));
        nested.Add(new EmployeeLeaf("C"));

        root.Add(nested);
        root.Add(new EmployeeLeaf("D"));

        return root;
    }

    private sealed class LoggingTransparentUnit : IOrganizationUnit
    {
        private readonly int _headcount;
        private readonly IList<string> _callOrder;

        public LoggingTransparentUnit(string name, int headcount, IList<string> callOrder)
        {
            Name = name;
            _headcount = headcount;
            _callOrder = callOrder;
        }

        public string Name { get; }

        public void Add(IOrganizationUnit unit) => throw new NotSupportedException("Logging unit does not support children.");

        public void Remove(IOrganizationUnit unit) => throw new NotSupportedException("Logging unit does not support children.");

        public int Headcount()
        {
            _callOrder.Add(Name);
            return _headcount;
        }
    }
}