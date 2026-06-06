namespace Behavioral.Iterator.FailFastIterator.Api;

public sealed class FailFastBag
{
    private readonly List<string> items = [];
    private int version;

    public void Add(string item)
    {
        items.Add(item);
        version++;
    }

    public IEnumerable<string> Iterate()
    {
        var snapshot = version;
        foreach (var item in items)
        {
            if (snapshot != version)
            {
                throw new InvalidOperationException("Collection modified during iteration.");
            }

            yield return item;
        }
    }
}

public static class FailFastIteratorDemo
{
    public static object Create()
    {
        var bag = new FailFastBag();
        bag.Add("A");
        bag.Add("B");

        var visited = bag.Iterate().ToArray();

        return new
        {
            Pattern = "Iterator",
            Variant = "Fail-fast Iterator",
            Visited = visited
        };
    }
}
