namespace Structural.Facade.LayeredFacade.Api;

public sealed class DataLayerFacade
{
    public IReadOnlyList<string> FetchOpenTickets() => new[] { "T-100", "T-204", "T-301" };
}

public sealed class DomainLayerFacade
{
    private readonly DataLayerFacade _data;

    public DomainLayerFacade(DataLayerFacade data) => _data = data;

    public object BuildBacklogSnapshot() => new
    {
        OpenCount = _data.FetchOpenTickets().Count,
        IsHealthy = _data.FetchOpenTickets().Count <= 5
    };
}

public sealed class ApiLayerFacade
{
    private readonly DomainLayerFacade _domain;

    public ApiLayerFacade(DomainLayerFacade domain) => _domain = domain;

    public object GetDashboard() => new
    {
        Team = "Platform",
        Snapshot = _domain.BuildBacklogSnapshot()
    };
}

public static class PatternDemo
{
    public static object Create()
    {
        var dashboard = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade())).GetDashboard();

        return new
        {
            Pattern = "Facade",
            Variant = "Layered Facade",
            Dashboard = dashboard
        };
    }
}
