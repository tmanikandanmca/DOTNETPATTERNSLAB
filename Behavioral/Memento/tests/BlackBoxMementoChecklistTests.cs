using System.Reflection;
using Behavioral.Memento.BlackBoxMemento.Api;

namespace Behavioral.Memento.Tests;

public class BlackBoxMementoChecklistTests
{
    [Test]
    public void Snapshots_RestoreTheExactPriorState()
    {
        var editor = new Editor();
        editor.Type("Draft");
        var snapshot = editor.Save();

        editor.Type(" v2");
        editor.Restore(snapshot);

        Assert.That(editor.Content, Is.EqualTo("Draft"));
    }

    [Test]
    public void Caretaker_CannotMutateOriginatorInternalsDirectly()
    {
        var contentProperty = typeof(Editor).GetProperty(nameof(Editor.Content));

        Assert.That(contentProperty, Is.Not.Null);
        Assert.That(contentProperty!.SetMethod, Is.Not.Null);
        Assert.That(contentProperty.SetMethod!.IsPublic, Is.False);
    }

    [Test]
    public void RestoreBehavior_IsRepeatableAcrossMultipleCycles()
    {
        var editor = new Editor();

        for (var i = 0; i < 5; i++)
        {
            editor.Type($"v{i}");
            var checkpoint = editor.Save();
            var expected = editor.Content;

            editor.Type("-delta");
            editor.Restore(checkpoint);

            Assert.That(editor.Content, Is.EqualTo(expected));
        }
    }

    [Test]
    public void BoundedCaretaker_StoresMementosWithFixedCapacity()
    {
        var editor = new Editor();
        var history = new BoundedCaretaker(capacity: 2);

        editor.Type("A");
        history.Push(editor.Save());

        editor.Type("B");
        history.Push(editor.Save());

        editor.Type("C");
        history.Push(editor.Save());

        Assert.That(history.Count, Is.EqualTo(2));

        editor.Restore(history.PopLatest());
        Assert.That(editor.Content, Is.EqualTo("ABC"));

        editor.Restore(history.PopLatest());
        Assert.That(editor.Content, Is.EqualTo("AB"));
    }

    [Test]
    public void SensitiveState_RemainsHiddenInBlackBoxSnapshots()
    {
        var snapshotType = typeof(Editor).Assembly
            .GetTypes()
            .Single(t => t.Name == "Snapshot" && t.DeclaringType == typeof(Editor));

        var publicReadableMembers = snapshotType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.GetMethod is { IsPublic: true })
            .ToList();

        Assert.That(publicReadableMembers, Is.Empty);
    }

    private sealed class BoundedCaretaker
    {
        private readonly int _capacity;
        private readonly List<IMemento> _items = new();

        public BoundedCaretaker(int capacity)
        {
            _capacity = capacity;
        }

        public int Count => _items.Count;

        public void Push(IMemento memento)
        {
            if (_items.Count == _capacity)
            {
                _items.RemoveAt(0);
            }

            _items.Add(memento);
        }

        public IMemento PopLatest()
        {
            var latestIndex = _items.Count - 1;
            var latest = _items[latestIndex];
            _items.RemoveAt(latestIndex);
            return latest;
        }
    }
}
