namespace Structural.Flyweight.CompositeFlyweight.Api;

public interface IPermissionFlyweight
{
    string Apply(string user);
}

public sealed class PermissionLeaf : IPermissionFlyweight
{
    public PermissionLeaf(string permission) => Permission = permission;

    public string Permission { get; }

    public string Apply(string user) => $"{user}:{Permission}";
}

public sealed class PermissionComposite : IPermissionFlyweight
{
    private readonly IReadOnlyList<IPermissionFlyweight> _children;

    public PermissionComposite(IEnumerable<IPermissionFlyweight> children) => _children = children.ToList();

    public string Apply(string user) => string.Join(", ", _children.Select(child => child.Apply(user)));
}

public sealed class PermissionFactory
{
    private readonly Dictionary<string, PermissionLeaf> _cache = new(StringComparer.OrdinalIgnoreCase);

    public PermissionLeaf Get(string permission)
    {
        if (!_cache.TryGetValue(permission, out var leaf))
        {
            leaf = new PermissionLeaf(permission);
            _cache[permission] = leaf;
        }

        return leaf;
    }

    public int SharedLeaves => _cache.Count;
}

public static class PatternDemo
{
    public static object Create()
    {
        var factory = new PermissionFactory();
        var composite = new PermissionComposite(new[]
        {
            factory.Get("read"),
            factory.Get("write"),
            factory.Get("read")
        });

        return new
        {
            Pattern = "Flyweight",
            Variant = "Composite Flyweight",
            Applied = composite.Apply("alex"),
            SharedLeafCount = factory.SharedLeaves
        };
    }
}
