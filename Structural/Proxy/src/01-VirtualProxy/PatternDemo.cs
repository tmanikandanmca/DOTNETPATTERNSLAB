namespace Structural.Proxy.VirtualProxy.Api;

public interface IDocument
{
    string Read();
}

public sealed class HeavyDocument : IDocument
{
    public static int CreatedCount;

    public HeavyDocument()
    {
        CreatedCount++;
        _content = $"Loaded at {DateTime.UtcNow:HH:mm:ss}";
    }

    private readonly string _content;

    public string Read() => _content;
}

public sealed class VirtualDocumentProxy : IDocument
{
    private HeavyDocument? _inner;

    public string Read()
    {
        _inner ??= new HeavyDocument();
        return _inner.Read();
    }
}

public static class PatternDemo
{
    public static object Create()
    {
        HeavyDocument.CreatedCount = 0;

        IDocument doc = new VirtualDocumentProxy();
        var first = doc.Read();
        var second = doc.Read();

        return new
        {
            Pattern = "Proxy",
            Variant = "Virtual Proxy",
            FirstRead = first,
            SecondRead = second,
            RealObjectCreatedCount = HeavyDocument.CreatedCount
        };
    }
}
