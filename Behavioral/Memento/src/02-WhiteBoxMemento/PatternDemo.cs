namespace Behavioral.Memento.WhiteBoxMemento.Api;

public sealed class WhiteBoxSnapshot
{
    public string Content { get; init; } = string.Empty;
}

public sealed class TextEditor
{
    public string Content { get; private set; } = string.Empty;

    public void Type(string text) => Content += text;

    public WhiteBoxSnapshot Save() => new() { Content = Content };

    public void Restore(WhiteBoxSnapshot snapshot) => Content = snapshot.Content;
}

public static class WhiteBoxMementoDemo
{
    public static object Create()
    {
        var editor = new TextEditor();
        editor.Type("Draft v1");

        var snapshot = editor.Save();
        editor.Type(" + additions");
        editor.Restore(snapshot);

        return new
        {
            Pattern = "Memento",
            Variant = "White-box Memento",
            SnapshotContent = snapshot.Content,
            Restored = editor.Content
        };
    }
}
