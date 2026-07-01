using Behavioral.Iterator.InternalIterator.Api;

namespace Behavioral.Iterator.UnitTests;

public class InternalIteratorChecklistTests
{
    [Test]
    public void Iterator_WorksWithoutLeakingCollectionInternals()
    {
        IInternalSequence sequence = new InternalSequence([2, 4, 6]);

        var visited = Consume(sequence);

        Assert.That(visited, Is.EqualTo(new[] { 2, 4, 6 }));
    }

    [Test]
    public void TraversalOrder_IsDeterministic_AndTestCovered()
    {
        IInternalSequence sequence = new InternalSequence([7, 3, 5]);

        var visited = Consume(sequence);

        Assert.That(visited, Is.EqualTo(new[] { 7, 3, 5 }));
    }

    [Test]
    public void NewIteratorStyles_ArePluggable_WithoutCallerRewrites()
    {
        IInternalSequence sequence = new DoublingSequence([1, 2, 3]);

        var visited = Consume(sequence);

        Assert.That(visited, Is.EqualTo(new[] { 2, 4, 6 }));
    }

    private static List<int> Consume(IInternalSequence sequence)
    {
        var visited = new List<int>();
        sequence.ForEach(visited.Add);
        return visited;
    }

    private sealed class DoublingSequence(IEnumerable<int> values) : IInternalSequence
    {
        public void ForEach(Action<int> action)
        {
            foreach (var value in values)
            {
                action(value * 2);
            }
        }
    }
}