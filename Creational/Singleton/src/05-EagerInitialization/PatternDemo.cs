namespace Singleton.EagerInitialization.Api;

public sealed class EagerInitializationService
{
    private static readonly EagerInitializationService Instance = new();
    private readonly Guid _id = Guid.NewGuid();

    static EagerInitializationService() { }

    private EagerInitializationService() { }

    public static EagerInitializationService GetInstance() => Instance;

    public string Id => _id.ToString("N");
}

public static class EagerInitializationDemo
{
    public static object Create()
    {
        var first = EagerInitializationService.GetInstance();
        var second = EagerInitializationService.GetInstance();

        return new
        {
            Pattern = "Singleton",
            Variant = "Eager Initialization",
            SameInstance = ReferenceEquals(first, second),
            FirstId = first.Id,
            SecondId = second.Id
        };
    }
}
