namespace Structural.Composite.SafeComposite.Api;

public interface INode
{
    string Name { get; }
    int TotalSize();
}

public sealed class FileNode : INode
{
    public FileNode(string name, int size)
    {
        Name = name;
        Size = size;
    }

    public string Name { get; }
    public int Size { get; }

    public int TotalSize() => Size;
}

public sealed class FolderNode : INode
{
    private readonly List<INode> _children = new();

    public FolderNode(string name) => Name = name;

    public string Name { get; }

    public void Add(INode child) => _children.Add(child);

    public int TotalSize() => _children.Sum(child => child.TotalSize());
}

public static class PatternDemo
{
    public static object Create()
    {
        var root = new FolderNode("root");
        root.Add(new FileNode("a.txt", 12));
        root.Add(new FileNode("b.log", 30));

        return new
        {
            Pattern = "Composite",
            Variant = "Safe Composite",
            Node = root.Name,
            Size = root.TotalSize()
        };
    }
}
