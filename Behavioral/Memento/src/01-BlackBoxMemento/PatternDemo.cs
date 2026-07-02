namespace Behavioral.Memento.BlackBoxMemento.Api;

public interface IMemento;

public sealed class Editor
{
    public string Content { get; private set; } = string.Empty;

    public void Type(string text) => Content += text;

    public IMemento Save() => new Snapshot(Content);

    public void Restore(IMemento memento)
    {
        if (memento is not Snapshot snapshot)
        {
            throw new ArgumentException("Memento was not created by this originator.", nameof(memento));
        }

        Content = snapshot.Content;
    }

    private sealed class Snapshot : IMemento
    {
        internal Snapshot(string content)
        {
            Content = content;
        }

        internal string Content { get; }
    }
}

public static class MementoDemo
{
    public static object Create()
    {
        var editor = new Editor();
        editor.Type("Hello");
        var snapshot = editor.Save();
        editor.Type(", world");
        editor.Restore(snapshot);

        return new
        {
            Pattern = "Memento",
            Variant = "Black-box Memento",
            Restored = editor.Content
        };
    }
}
