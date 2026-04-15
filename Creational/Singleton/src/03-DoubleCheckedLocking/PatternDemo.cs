namespace Singleton.DoubleCheckedLocking.Api;

public sealed class DoubleCheckedLockingService
{
    private static DoubleCheckedLockingService? _instance;
    private static readonly object SyncRoot = new();
    private readonly Guid _id = Guid.NewGuid();

    private DoubleCheckedLockingService() { }

    public static DoubleCheckedLockingService GetInstance()
    {
        if (_instance is null)
        {
            lock (SyncRoot)
            {
                _instance ??= new DoubleCheckedLockingService();
            }
        }

        return _instance!;
    }

    public string Id => _id.ToString("N");
}

public static class DoubleCheckedLockingDemo
{
    public static object Create()
    {
        var first = DoubleCheckedLockingService.GetInstance();
        var second = DoubleCheckedLockingService.GetInstance();

        return new
        {
            Pattern = "Singleton",
            Variant = "Double-Checked Locking",
            SameInstance = ReferenceEquals(first, second),
            FirstId = first.Id,
            SecondId = second.Id
        };
    }
}
