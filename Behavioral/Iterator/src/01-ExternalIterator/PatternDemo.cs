namespace Behavioral.Iterator.ExternalIterator.Api;

public sealed class NumberCollection(IEnumerable<int> values)
{
    private readonly List<int> numbers = values.ToList();

    public ExternalNumberIterator GetIterator() => new(numbers);

    public IReadOnlyList<int> InternalTraverse() => numbers.Where(number => number % 2 == 0).ToArray();
}

public sealed class ExternalNumberIterator(IReadOnlyList<int> numbers)
{
    private int index = -1;

    public bool MoveNext() => ++index < numbers.Count;

    public int Current => numbers[index];
}

public sealed class FailFastCollection
{
    private readonly List<string> items = [];
    private int version;

    public void Add(string item)
    {
        items.Add(item);
        version++;
    }

    public IEnumerable<string> Enumerate()
    {
        var snapshotVersion = version;
        foreach (var item in items)
        {
            if (snapshotVersion != version)
            {
                throw new InvalidOperationException("Collection changed during iteration.");
            }

            yield return item;
        }
    }
}

public static class IteratorDemo
{
    public static object Create()
    {
        var collection = new NumberCollection([1, 2, 3, 4, 5]);
        var iterator = collection.GetIterator();
        var external = new List<int>();

        while (iterator.MoveNext())
        {
            external.Add(iterator.Current);
        }

        var failFast = new FailFastCollection();
        failFast.Add("A");
        failFast.Add("B");

        return new
        {
            Pattern = "Iterator",
            Internal = collection.InternalTraverse(),
            External = external,
            FailFast = failFast.Enumerate().ToArray()
        };
    }
}
