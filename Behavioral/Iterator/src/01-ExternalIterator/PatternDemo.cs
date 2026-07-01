namespace Behavioral.Iterator.ExternalIterator.Api;

public interface INumberSequence
{
    INumberIterator GetIterator();
}

public interface INumberIterator
{
    bool MoveNext();
    int Current { get; }
}

public sealed class NumberCollection(IEnumerable<int> values) : INumberSequence
{
    private readonly List<int> numbers = values.ToList();

    public INumberIterator GetIterator() => new ExternalNumberIterator(numbers);

    public IReadOnlyList<int> InternalTraverse() => numbers.Where(number => number % 2 == 0).ToArray();
}

public sealed class ExternalNumberIterator(IReadOnlyList<int> numbers) : INumberIterator
{
    private int index = -1;

    public bool MoveNext() => ++index < numbers.Count;

    public int Current => numbers[index];
}

public static class IteratorDemo
{
    public static object Create()
    {
        INumberSequence collection = new NumberCollection([1, 2, 3, 4, 5]);
        var external = new List<int>();
        var iterator = collection.GetIterator();

        while (iterator.MoveNext())
        {
            external.Add(iterator.Current);
        }

        return new
        {
            Pattern = "Iterator",
            Variant = "External Iterator",
            External = external
        };
    }
}
