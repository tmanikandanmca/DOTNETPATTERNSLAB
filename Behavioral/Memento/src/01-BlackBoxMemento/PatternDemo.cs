namespace Behavioral.Memento.BlackBoxMemento.Api;

public sealed record Snapshot(string Content);

public sealed class Editor
{
    public string Content { get; private set; } = string.Empty;

    public void Type(string text) => Content += text;

    public Snapshot Save() => new(Content);

    public void Restore(Snapshot snapshot) => Content = snapshot.Content;
}

public sealed class WhiteBoxSnapshot
{
    public WhiteBoxSnapshot(string content) => Content = content;

    public string Content { get; }
}

public sealed class WhiteBoxEditor
{
    public string Content { get; private set; } = string.Empty;

    public void Type(string text) => Content += text;

    public WhiteBoxSnapshot Save() => new(Content);

    public void Restore(WhiteBoxSnapshot snapshot) => Content = snapshot.Content;
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

        var whiteBoxEditor = new WhiteBoxEditor();
        whiteBoxEditor.Type("Draft");
        var whiteSnapshot = whiteBoxEditor.Save();
        whiteBoxEditor.Type(" v2");
        whiteBoxEditor.Restore(whiteSnapshot);

        return new
        {
            Pattern = "Memento",
            BlackBox = editor.Content,
            WhiteBox = whiteBoxEditor.Content
        };
    }
}
