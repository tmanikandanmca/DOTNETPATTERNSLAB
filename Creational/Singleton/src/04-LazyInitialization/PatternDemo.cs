namespace Singleton.LazyInitialization.Api;

public sealed class LazyInitializationService
{
    private static readonly Lazy<LazyInitializationService> Instance = new(() => new LazyInitializationService());
    private readonly Guid _id = Guid.NewGuid();

    private LazyInitializationService() { }

    public static LazyInitializationService GetInstance() => Instance.Value;

    public string Id => _id.ToString("N");
    public string State { get; private set; } = "Initial";

    public void SetState(string state)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(state);
        State = state;
    }
}

public static class LazyInitializationDemo
{
    public static object Create()
    {
        var first = LazyInitializationService.GetInstance();
        var second = LazyInitializationService.GetInstance();

        return new
        {
            Pattern = "Singleton",
            Variant = "Lazy Initialization",
            SameInstance = ReferenceEquals(first, second),
            FirstId = first.Id,
            SecondId = second.Id
        };
    }
}
