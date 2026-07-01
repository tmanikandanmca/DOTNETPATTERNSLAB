using Behavioral.Iterator.ExternalIterator.Api;

namespace Behavioral.Iterator.UnitTests;

public class ExternalIteratorChecklistTests
{
    [Test]
    public void Iterator_WorksWithoutLeakingCollectionInternals()
    {
        INumberSequence sequence = new NumberCollection([1, 2, 3]);

        var visited = Consume(sequence);

        Assert.That(visited, Is.EqualTo(new[] { 1, 2, 3 }));
    }

    [Test]
    public void TraversalOrder_IsDeterministic_AndTestCovered()
    {
        INumberSequence sequence = new NumberCollection([4, 1, 9, 2]);

        var visited = Consume(sequence);

        Assert.That(visited, Is.EqualTo(new[] { 4, 1, 9, 2 }));
    }

    [Test]
    public void ExternalIterator_PreservesStateAcrossCalls()
    {
        INumberSequence sequence = new NumberCollection([10, 20, 30]);
        var iterator = sequence.GetIterator();

        Assert.Multiple(() =>
        {
            Assert.That(iterator.MoveNext(), Is.True);
            Assert.That(iterator.Current, Is.EqualTo(10));
            Assert.That(iterator.MoveNext(), Is.True);
            Assert.That(iterator.Current, Is.EqualTo(20));
            Assert.That(iterator.MoveNext(), Is.True);
            Assert.That(iterator.Current, Is.EqualTo(30));
            Assert.That(iterator.MoveNext(), Is.False);
        });
    }

    [Test]
    public void NewIteratorStyles_ArePluggable_WithoutCallerRewrites()
    {
        INumberSequence sequence = new ReverseNumberSequence([1, 2, 3]);

        var visited = Consume(sequence);

        Assert.That(visited, Is.EqualTo(new[] { 3, 2, 1 }));
    }

    private static List<int> Consume(INumberSequence sequence)
    {
        var visited = new List<int>();
        var iterator = sequence.GetIterator();

        while (iterator.MoveNext())
        {
            visited.Add(iterator.Current);
        }

        return visited;
    }

    private sealed class ReverseNumberSequence(IEnumerable<int> numbers) : INumberSequence
    {
        public INumberIterator GetIterator() => new ExternalNumberIterator(numbers.Reverse().ToArray());
    }
}