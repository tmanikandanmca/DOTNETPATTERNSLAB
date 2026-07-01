using Behavioral.Iterator.FailFastIterator.Api;

namespace Behavioral.Iterator.UnitTests;

public class FailFastIteratorChecklistTests
{
    [Test]
    public void Iterator_WorksWithoutLeakingCollectionInternals()
    {
        IStringSequence sequence = BuildBag("A", "B");

        var visited = sequence.Iterate().ToArray();

        Assert.That(visited, Is.EqualTo(new[] { "A", "B" }));
    }

    [Test]
    public void TraversalOrder_IsDeterministic_AndTestCovered()
    {
        IStringSequence sequence = BuildBag("first", "second", "third");

        var visited = sequence.Iterate().ToArray();

        Assert.That(visited, Is.EqualTo(new[] { "first", "second", "third" }));
    }

    [Test]
    public void FailFastBehavior_IsValidated_UnderMutationScenarios()
    {
        var bag = BuildBag("A", "B", "C");
        using var iterator = bag.Iterate().GetEnumerator();

        Assert.That(iterator.MoveNext(), Is.True);
        Assert.That(iterator.Current, Is.EqualTo("A"));

        bag.Add("D");

        var error = Assert.Throws<InvalidOperationException>(() => iterator.MoveNext());
        Assert.That(error!.Message, Is.EqualTo("Collection modified during iteration."));
    }

    [Test]
    public void NewIteratorStyles_ArePluggable_WithoutCallerRewrites()
    {
        IStringSequence sequence = new SnapshotSequence(["X", "Y"]);

        var visited = sequence.Iterate().ToArray();

        Assert.That(visited, Is.EqualTo(new[] { "X", "Y" }));
    }

    private static FailFastBag BuildBag(params string[] items)
    {
        var bag = new FailFastBag();
        foreach (var item in items)
        {
            bag.Add(item);
        }

        return bag;
    }

    private sealed class SnapshotSequence(IEnumerable<string> items) : IStringSequence
    {
        public IEnumerable<string> Iterate()
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }
}