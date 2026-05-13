namespace Structural.Facade.SimpleFacade.Api;

public sealed class InventoryService
{
    public bool Reserve(string sku, int quantity) => quantity > 0 && sku.Length > 0;
}

public sealed class BillingService
{
    public string Charge(decimal amount) => $"PAY-{(int)(amount * 100):000000}";
}

public sealed class ShippingService
{
    public string CreateShipment(string sku) => $"SHIP-{sku.ToUpperInvariant()}";
}

public sealed class OrderFacade
{
    private readonly InventoryService _inventory = new();
    private readonly BillingService _billing = new();
    private readonly ShippingService _shipping = new();

    public object PlaceOrder(string sku, int quantity, decimal price)
    {
        var reserved = _inventory.Reserve(sku, quantity);
        var paymentId = _billing.Charge(quantity * price);
        var shipmentId = _shipping.CreateShipment(sku);

        return new { reserved, paymentId, shipmentId };
    }
}

public static class PatternDemo
{
    public static object Create()
    {
        var facade = new OrderFacade();
        var result = facade.PlaceOrder("kbd-01", 2, 49.5m);

        return new
        {
            Pattern = "Facade",
            Variant = "Simple Facade",
            Result = result
        };
    }
}
