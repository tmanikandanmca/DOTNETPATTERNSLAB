using Behavioral.Memento.WhiteBoxMemento.Api;

namespace Behavioral.Memento.Tests;

public class WhiteBoxMementoChecklistTests
{
    [Test]
    public void Snapshots_RestoreTheExactPriorState()
    {
        var editor = new TextEditor();
        editor.Type("Draft");
        var snapshot = editor.Save();

        editor.Type(" v2");
        editor.Restore(snapshot);

        Assert.That(editor.Content, Is.EqualTo("Draft"));
    }

    [Test]
    public void Caretaker_CanInspectSnapshotInWhiteBoxVariant()
    {
        var editor = new TextEditor();
        editor.Type("release-notes");

        var snapshot = editor.Save();

        Assert.That(snapshot.Content, Is.EqualTo("release-notes"));
    }

    [Test]
    public void RestoreBehavior_IsRepeatableAcrossMultipleCycles()
    {
        var editor = new TextEditor();

        for (var i = 0; i < 5; i++)
        {
            editor.Type($"{i}");
            var checkpoint = editor.Save();
            var expected = editor.Content;

            editor.Type("+");
            editor.Restore(checkpoint);

            Assert.That(editor.Content, Is.EqualTo(expected));
        }
    }

    [Test]
    public void Caretaker_CanIntentionallyRetainFullHistory()
    {
        var editor = new TextEditor();
        var retained = new List<WhiteBoxSnapshot>();

        editor.Type("A");
        retained.Add(editor.Save());

        editor.Type("B");
        retained.Add(editor.Save());

        editor.Type("C");
        retained.Add(editor.Save());

        Assert.That(retained.Select(s => s.Content), Is.EqualTo(new[] { "A", "AB", "ABC" }));
    }
}
