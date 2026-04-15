namespace Singleton.ThreadSafeLock.Api;

public sealed class ThreadSafeLockService
{
    private static ThreadSafeLockService? _instance;
    private static readonly object SyncRoot = new();
    private readonly Guid _id = Guid.NewGuid();

    private ThreadSafeLockService() { }

    public static ThreadSafeLockService GetInstance()
    {
        lock (SyncRoot)
        {
            _instance ??= new ThreadSafeLockService();
            return _instance;
        }
    }

    public string Id => _id.ToString("N");
}

public static class ThreadSafeLockDemo
{
    public static object Create()
    {
        var first = ThreadSafeLockService.GetInstance();
        var second = ThreadSafeLockService.GetInstance();

        return new
        {
            Pattern = "Singleton",
            Variant = "Thread-Safe (lock)",
            SameInstance = ReferenceEquals(first, second),
            FirstId = first.Id,
            SecondId = second.Id
        };
    }
}
