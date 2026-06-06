namespace Behavioral.Iterator.InternalIterator.Api;

public sealed class InternalSequence(IEnumerable<int> values)
{
    private readonly List<int> items = values.ToList();

    public void ForEach(Action<int> action)
    {
        foreach (var item in items)
        {
            action(item);
        }
    }
}

public static class InternalIteratorDemo
{
    public static object Create()
    {
        var sequence = new InternalSequence([2, 4, 6, 8]);
        var processed = new List<int>();

        sequence.ForEach(value => processed.Add(value / 2));

        return new
        {
            Pattern = "Iterator",
            Variant = "Internal Iterator",
            Output = processed
        };
    }
}
