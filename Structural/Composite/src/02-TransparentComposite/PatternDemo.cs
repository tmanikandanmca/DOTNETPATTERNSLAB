namespace Structural.Composite.TransparentComposite.Api;

public interface IOrganizationUnit
{
    string Name { get; }
    void Add(IOrganizationUnit unit);
    void Remove(IOrganizationUnit unit);
    int Headcount();
}

public sealed class EmployeeLeaf : IOrganizationUnit
{
    public EmployeeLeaf(string name) => Name = name;

    public string Name { get; }

    public void Add(IOrganizationUnit unit) => throw new NotSupportedException("Leaf cannot add children.");

    public void Remove(IOrganizationUnit unit) => throw new NotSupportedException("Leaf cannot remove children.");

    public int Headcount() => 1;
}

public sealed class TeamComposite : IOrganizationUnit
{
    private readonly List<IOrganizationUnit> _children = new();

    public TeamComposite(string name) => Name = name;

    public string Name { get; }

    public void Add(IOrganizationUnit unit) => _children.Add(unit);

    public void Remove(IOrganizationUnit unit) => _children.Remove(unit);

    public int Headcount() => _children.Sum(child => child.Headcount());
}

public static class PatternDemo
{
    public static object Create()
    {
        var team = new TeamComposite("Payments");
        team.Add(new EmployeeLeaf("Nila"));
        team.Add(new EmployeeLeaf("Arun"));

        var leafError = string.Empty;
        try
        {
            IOrganizationUnit leaf = new EmployeeLeaf("Isha");
            leaf.Add(new EmployeeLeaf("ShouldFail"));
        }
        catch (NotSupportedException ex)
        {
            leafError = ex.Message;
        }

        return new
        {
            Pattern = "Composite",
            Variant = "Transparent Composite",
            Team = team.Name,
            Headcount = team.Headcount(),
            LeafOperationBehavior = leafError
        };
    }
}
