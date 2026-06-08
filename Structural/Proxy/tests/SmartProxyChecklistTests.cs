using Structural.Proxy.SmartProxy.Api;

namespace Structural.Proxy.Tests;

public class SmartProxyChecklistTests
{
    [Test]
    public void ClientCode_WorksWithSubjectInterfaceOnly()
    {
        IStockService service = new SmartStockProxy(new StockService());

        var quantity = service.GetQuantity("kbd-01");

        Assert.That(quantity, Is.EqualTo(18));
    }

    [Test]
    public void ProxyWrapsCaching_WithoutChangingOutputContract()
    {
        StockService.Calls = 0;
        var proxy = new SmartStockProxy(new StockService());

        var first = proxy.GetQuantity("kbd-01");
        var second = proxy.GetQuantity("kbd-01");

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.EqualTo(18));
            Assert.That(second, Is.EqualTo(18));
            Assert.That(StockService.Calls, Is.EqualTo(1));
            Assert.That(proxy.CacheHits, Is.EqualTo(1));
        });
    }

    [Test]
    public void CacheWrapsRealSubject_ForRepeatedLookups()
    {
        StockService.Calls = 0;
        var proxy = new SmartStockProxy(new StockService());

        var quantities = new[]
        {
            proxy.GetQuantity("kbd-01"),
            proxy.GetQuantity("mse-02"),
            proxy.GetQuantity("kbd-01")
        };

        Assert.Multiple(() =>
        {
            Assert.That(quantities, Is.EqualTo(new[] { 18, 7, 18 }));
            Assert.That(StockService.Calls, Is.EqualTo(2));
            Assert.That(proxy.CacheHits, Is.EqualTo(1));
        });
    }

    [Test]
    public void ProxyAddsMetrics_WithoutChangingReturnValues()
    {
        StockService.Calls = 0;
        var proxy = new SmartStockProxy(new StockService());

        var result = proxy.GetQuantity("unknown-sku");

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(0));
            Assert.That(StockService.Calls, Is.EqualTo(1));
        });
    }
}