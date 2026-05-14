using Structural.Facade.SimpleFacade.Api;
using Structural.Facade.LayeredFacade.Api;

namespace Facade.UnitTests;

public class FacadeChecklistTests
{
    [Test]
    public void SimpleFacade_SubsystemsHandleHappyAndEdgeInputs()
    {
        var inventory = new InventoryService();
        var billing = new BillingService();
        var shipping = new ShippingService();

        Assert.Multiple(() =>
        {
            Assert.That(inventory.Reserve("kbd-01", 2), Is.True);
            Assert.That(inventory.Reserve("", 2), Is.False);
            Assert.That(inventory.Reserve("kbd-01", 0), Is.False);
            Assert.That(billing.Charge(99m), Is.EqualTo("PAY-009900"));
            Assert.That(shipping.CreateShipment("kbd-01"), Is.EqualTo("SHIP-KBD-01"));
        });
    }

    [Test]
    public void SimpleFacade_OrchestratesSubsystemCalls_IntoSingleResult()
    {
        var result = new OrderFacade().PlaceOrder("kbd-01", 2, 49.5m);

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(result, "reserved"), Is.EqualTo(true));
            Assert.That(GetProp(result, "paymentId"), Is.EqualTo("PAY-009900"));
            Assert.That(GetProp(result, "shipmentId"), Is.EqualTo("SHIP-KBD-01"));
        });
    }

    [Test]
    public void LayeredFacade_ComposesDataAcrossLayers()
    {
        var dashboard = new ApiLayerFacade(new DomainLayerFacade(new DataLayerFacade())).GetDashboard();
        var snapshot = GetProp(dashboard, "Snapshot")!;

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(dashboard, "Team"), Is.EqualTo("Platform"));
            Assert.That(GetProp(snapshot, "OpenCount"), Is.EqualTo(3));
            Assert.That(GetProp(snapshot, "IsHealthy"), Is.EqualTo(true));
        });
    }

    [Test]
    public void PatternDemos_ReturnExpectedFacadePayloads()
    {
        var simple = Structural.Facade.SimpleFacade.Api.PatternDemo.Create();
        var layered = Structural.Facade.LayeredFacade.Api.PatternDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(simple, "Variant"), Is.EqualTo("Simple Facade"));
            Assert.That(GetProp(simple, "Result"), Is.Not.Null);

            Assert.That(GetProp(layered, "Variant"), Is.EqualTo("Layered Facade"));
            Assert.That(GetProp(layered, "Dashboard"), Is.Not.Null);
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
