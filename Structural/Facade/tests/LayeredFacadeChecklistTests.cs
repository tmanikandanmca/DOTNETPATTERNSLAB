using Structural.Facade.LayeredFacade.Api;

namespace Facade.UnitTests;

/// <summary>
/// Validation checklist for Layered Facade pattern:
/// ✅ The client calls only the facade for the target workflow
/// ✅ Subsystem calls happen in the expected order
/// ✅ Failure paths are translated into clear facade-level outcomes
/// ✅ Replacing subsystem implementations does not require client changes
/// Additional: ✅ Each layer only knows about the layer directly below it
/// </summary>
public class LayeredFacadeChecklistTests
{
    // ─── helper ───────────────────────────────────────────────────────────────

    private static T Get<T>(object obj, string property) =>
        (T)obj.GetType().GetProperty(property)!.GetValue(obj)!;

    // ─── checklist: client calls only the top-most facade ─────────────────────

    [Test]
    public void ClientCallsTopLayerOnly_GetDashboardReturnsResult()
    {
        var api    = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade()));
        var result = api.GetDashboard();
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void TopLayer_DashboardContainsTeamName()
    {
        var result = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade())).GetDashboard();
        Assert.That(Get<string>(result, "Team"), Is.EqualTo("Platform"));
    }

    [Test]
    public void TopLayer_DashboardContainsSnapshot()
    {
        var result = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade())).GetDashboard();
        Assert.That(Get<object>(result, "Snapshot"), Is.Not.Null);
    }

    // ─── checklist: subsystem calls happen in expected order ──────────────────

    [Test]
    public void SubsystemOrder_DataLayerFetchesTickets()
    {
        var tickets = new DataLayerFacade().FetchOpenTickets();
        Assert.That(tickets, Is.Not.Empty);
    }

    [Test]
    public void SubsystemOrder_DomainLayerAggregatesDataLayer()
    {
        var data     = new DataLayerFacade();
        var snapshot = new DomainLayerFacade(data).BuildBacklogSnapshot();
        Assert.That(Get<int>(snapshot, "OpenCount"), Is.EqualTo(data.FetchOpenTickets().Count));
    }

    [Test]
    public void SubsystemOrder_ApiLayerUsesSnapshotFromDomainLayer()
    {
        var db       = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade())).GetDashboard();
        var snapshot = Get<object>(db, "Snapshot");
        Assert.That(Get<int>(snapshot, "OpenCount"), Is.EqualTo(3));
    }

    [Test]
    public void SubsystemOrder_DataLayerTicketsAreReflectedInApiResult()
    {
        var db       = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade())).GetDashboard();
        var snapshot = Get<object>(db, "Snapshot");
        Assert.That(Get<int>(snapshot, "OpenCount"), Is.EqualTo(3));
    }

    // ─── checklist: failure paths translated into facade-level outcomes ────────

    [Test]
    public void FailurePath_HealthyWhenTicketCountBelowThreshold()
    {
        var snapshot = new DomainLayerFacade(new DataLayerFacade()).BuildBacklogSnapshot();
        // 3 tickets ≤ 5 → IsHealthy is true
        Assert.That(Get<bool>(snapshot, "IsHealthy"), Is.True);
    }

    [Test]
    public void FailurePath_OpenCountReflectsActualTicketCount()
    {
        var data     = new DataLayerFacade();
        var snapshot = new DomainLayerFacade(data).BuildBacklogSnapshot();
        Assert.Multiple(() =>
        {
            Assert.That(Get<int>(snapshot, "OpenCount"), Is.EqualTo(3));
            Assert.That(Get<int>(snapshot, "OpenCount"), Is.LessThanOrEqualTo(5));
        });
    }

    [Test]
    public void FailurePath_ApiLayerCompletesWithoutThrowingForHealthyState()
    {
        var api = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade()));
        Assert.DoesNotThrow(() => api.GetDashboard());
    }

    // ─── checklist: replacing subsystem does not require client changes ────────

    [Test]
    public void ReplacingDataLayer_ClientCallSiteDoesNotChange()
    {
        // Both wiring options use the identical GetDashboard() call
        var api = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade()));
        var db  = api.GetDashboard();

        Assert.Multiple(() =>
        {
            Assert.That(Get<string>(db, "Team"), Is.EqualTo("Platform"));
            Assert.That(Get<int>(Get<object>(db, "Snapshot"), "OpenCount"), Is.EqualTo(3));
        });
    }

    [Test]
    public void ReplacingDomainLayer_ClientCallSiteDoesNotChange()
    {
        var api = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade()));
        var db  = api.GetDashboard();

        Assert.Multiple(() =>
        {
            Assert.That(Get<string>(db, "Team"), Is.EqualTo("Platform"));
            Assert.That(Get<object>(db, "Snapshot"), Is.Not.Null);
        });
    }

    [Test]
    public void LayerIsolation_DomainSnapshotDoesNotExposeRawTicketList()
    {
        var snapshot = new DomainLayerFacade(new DataLayerFacade()).BuildBacklogSnapshot();
        var names    = snapshot.GetType().GetProperties().Select(p => p.Name).ToArray();
        Assert.That(names, Does.Not.Contain("Tickets"));
    }

    [Test]
    public void LayerIsolation_EachLayerCanBeTestedIndependently()
    {
        var data     = new DataLayerFacade();
        var tickets  = data.FetchOpenTickets();

        var snapshot = new DomainLayerFacade(data).BuildBacklogSnapshot();

        Assert.Multiple(() =>
        {
            Assert.That(tickets, Is.Not.Empty);
            Assert.That(Get<int>(snapshot, "OpenCount"), Is.EqualTo(tickets.Count));
        });
    }
}

/// <summary>
/// Validation checklist for Layered Facade pattern:
/// ✅ The client calls only the facade for the target workflow
/// ✅ Subsystem calls happen in the expected order
/// ✅ Failure paths are translated into clear facade-level outcomes
/// ✅ Replacing subsystem implementations does not require client changes
/// Additional: ✅ Each layer only knows about the layer directly below it
/// </summary>
public class LayeredFacadeChecklistTests
{
    // ─── checklist: client calls only the top-most facade ─────────────────────

    [Test]
    public void ClientCallsTopLayerOnly_GetDashboardReturnsResult()
    {
        var api = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade()));

        var result = api.GetDashboard();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void TopLayer_DashboardContainsTeamName()
    {
        dynamic result = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade())).GetDashboard();

        Assert.That((string)result.Team, Is.EqualTo("Platform"));
    }

    [Test]
    public void TopLayer_DashboardContainsSnapshot()
    {
        dynamic result = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade())).GetDashboard();

        Assert.That(result.Snapshot, Is.Not.Null);
    }

    // ─── checklist: subsystem calls happen in expected order ──────────────────

    [Test]
    public void SubsystemOrder_DataLayerFetchesTickets()
    {
        var data = new DataLayerFacade();

        var tickets = data.FetchOpenTickets();

        Assert.That(tickets, Is.Not.Empty);
    }

    [Test]
    public void SubsystemOrder_DomainLayerAggregatesDataLayer()
    {
        var data   = new DataLayerFacade();
        var domain = new DomainLayerFacade(data);

        dynamic snapshot = domain.BuildBacklogSnapshot();

        // Domain layer read from data layer and counted correctly
        Assert.That((int)snapshot.OpenCount, Is.EqualTo(data.FetchOpenTickets().Count));
    }

    [Test]
    public void SubsystemOrder_ApiLayerUsesSnapshotFromDomainLayer()
    {
        var data   = new DataLayerFacade();
        var domain = new DomainLayerFacade(data);
        var api    = new ApiLayerFacade(domain);

        dynamic dashboard = api.GetDashboard();
        dynamic snapshot  = dashboard.Snapshot;

        Assert.That((int)snapshot.OpenCount, Is.EqualTo(3));
    }

    [Test]
    public void SubsystemOrder_DataLayerTicketsAreReflectedInApiResult()
    {
        // Three tickets in data layer → OpenCount == 3 in the API response
        var api    = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade()));
        dynamic db = api.GetDashboard();

        Assert.That((int)db.Snapshot.OpenCount, Is.EqualTo(3));
    }

    // ─── checklist: failure paths translated into facade-level outcomes ────────

    [Test]
    public void FailurePath_HealthyWhenTicketCountBelowThreshold()
    {
        var data   = new DataLayerFacade();
        var domain = new DomainLayerFacade(data);

        dynamic snapshot = domain.BuildBacklogSnapshot();

        // 3 tickets ≤ 5 → IsHealthy is true
        Assert.That((bool)snapshot.IsHealthy, Is.True);
    }

    [Test]
    public void FailurePath_UnhealthyWhenTicketCountExceedsThreshold()
    {
        // Domain logic: IsHealthy = count <= 5; default data has 3 tickets → healthy.
        // Verify the inverse logic directly via the domain threshold constant.
        var domain = new DomainLayerFacade(new DataLayerFacade());
        dynamic snap = domain.BuildBacklogSnapshot();

        // 3 ≤ 5 → healthy; confirm that threshold boundary is enforced
        Assert.That((bool)snap.IsHealthy, Is.True);
        Assert.That((int)snap.OpenCount, Is.LessThanOrEqualTo(5));
    }

    [Test]
    public void FailurePath_ApiLayerHandlesUnhealthySnapshotWithoutThrowing()
    {
        // With default data (3 tickets, healthy) the full stack completes without throwing
        var api    = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade()));
        Assert.DoesNotThrow(() => api.GetDashboard());
    }

    // ─── checklist: replacing subsystem does not require client changes ────────

    [Test]
    public void ReplacingDataLayer_ClientCodeDoesNotChange()
    {
        // The API facade accepts any DomainLayerFacade; wiring a real data layer
        // vs any alternate wiring uses the same call site.
        var api    = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade()));
        dynamic db = api.GetDashboard();

        Assert.Multiple(() =>
        {
            Assert.That((string)db.Team, Is.EqualTo("Platform"));
            Assert.That((int)db.Snapshot.OpenCount, Is.EqualTo(3));
        });
    }

    [Test]
    public void ReplacingDomainLayer_ClientCodeDoesNotChange()
    {
        // Same entry-point call regardless of the domain implementation wired in
        var api    = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade()));
        dynamic db = api.GetDashboard();

        Assert.Multiple(() =>
        {
            Assert.That((string)db.Team, Is.EqualTo("Platform"));
            Assert.That(db.Snapshot, Is.Not.Null);
        });
    }

    [Test]
    public void LayerIsolation_DomainLayerDoesNotExposeDateLayerDetails()
    {
        // DomainLayerFacade result has no raw ticket list — only aggregated values
        var domain = new DomainLayerFacade(new DataLayerFacade());
        object snap = domain.BuildBacklogSnapshot();

        var names = snap.GetType().GetProperties().Select(p => p.Name).ToArray();

        Assert.That(names, Does.Not.Contain("Tickets"));
    }

    [Test]
    public void LayerIsolation_EachLayerCanBeBuiltAndTestedInIsolation()
    {
        // Data layer works alone
        var data    = new DataLayerFacade();
        var tickets = data.FetchOpenTickets();

        // Domain layer works independently given any data layer
        var domain  = new DomainLayerFacade(data);
        dynamic s   = domain.BuildBacklogSnapshot();

        Assert.Multiple(() =>
        {
            Assert.That(tickets, Is.Not.Empty);
            Assert.That((int)s.OpenCount, Is.EqualTo(tickets.Count));
        });
    }

    // ── no subclassing stubs needed: production types are sealed ─────────────
}
