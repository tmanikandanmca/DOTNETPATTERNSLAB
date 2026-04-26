namespace Singleton.BasicSingleton.Api;

public sealed class BasicSingletonService
{
    private static readonly BasicSingletonService Instance = new();
    private readonly Guid _id = Guid.NewGuid();

    private BasicSingletonService() { }

    public static BasicSingletonService GetInstance() => Instance;

    public string Id => _id.ToString("N");
    public string State { get; private set; } = "Initial";

    public void SetState(string state)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(state);
        State = state;
    }
}

public static class BasicSingletonDemo
{
    public static object Create()
    {
        var first = BasicSingletonService.GetInstance();
        var second = BasicSingletonService.GetInstance();

        return new
        {
            Pattern = "Singleton",
            Variant = "Basic Singleton",
            SameInstance = ReferenceEquals(first, second),
            FirstId = first.Id,
            SecondId = second.Id
        };
    }
}
