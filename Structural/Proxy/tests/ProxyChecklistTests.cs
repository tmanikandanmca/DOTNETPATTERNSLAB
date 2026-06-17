using Structural.Proxy.VirtualProxy.Api;
using Structural.Proxy.RemoteProxy.Api;
using Structural.Proxy.ProtectionProxy.Api;
using Structural.Proxy.SmartProxy.Api;

namespace Proxy.UnitTests;

public class ProxyChecklistTests
{
    [SetUp]
    public void ResetCounters()
    {
        HeavyDocument.CreatedCount = 0;
        StockService.Calls = 0;
    }

    [Test]
    public void VirtualProxy_LazilyCreatesRealObject_OnlyOnFirstUse()
    {
        IDocument doc = new VirtualDocumentProxy();

        Assert.That(HeavyDocument.CreatedCount, Is.EqualTo(0));

        var first = doc.Read();
        var second = doc.Read();

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.EqualTo(second));
            Assert.That(HeavyDocument.CreatedCount, Is.EqualTo(1));
        });
    }

    [Test]
    public void RemoteProxy_ForwardsCalls_AndAddsEndpointContext()
    {
        IWeatherService proxy = new RemoteWeatherProxy(new RemoteWeatherService(), "https://weather.example/v1");

        Assert.That(proxy.Forecast("Chennai"), Is.EqualTo("[https://weather.example/v1] Chennai: 30C, Clear"));
    }

    [Test]
    public void ProtectionProxy_AllowsAdmin_AndDeniesNonAdmin()
    {
        var report = new ConfidentialReport();
        IConfidentialReport admin = new ProtectionProxyReport(report, "admin");
        IConfidentialReport guest = new ProtectionProxyReport(report, "guest");

        Assert.Multiple(() =>
        {
            Assert.That(admin.Read(), Is.EqualTo("Quarterly margin: 27.3%"));
            Assert.That(guest.Read(), Is.EqualTo("Access denied"));
        });
    }

    [Test]
    public void SmartProxy_CachesResults_AndTracksHitsWithCaseInsensitiveKey()
    {
        IStockService proxy = new SmartStockProxy(new StockService());

        var first = proxy.GetQuantity("kbd-01");
        var second = proxy.GetQuantity("KBD-01");
        var third = proxy.GetQuantity("mse-02");

        var smart = (SmartStockProxy)proxy;

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.EqualTo(18));
            Assert.That(second, Is.EqualTo(18));
            Assert.That(third, Is.EqualTo(7));
            Assert.That(StockService.Calls, Is.EqualTo(2));
            Assert.That(smart.CacheHits, Is.EqualTo(1));
        });
    }

    [Test]
    public void PatternDemos_ReturnExpectedProxyPayloads()
    {
        var virtualProxy = Structural.Proxy.VirtualProxy.Api.PatternDemo.Create();
        var remoteProxy = Structural.Proxy.RemoteProxy.Api.PatternDemo.Create();
        var protectionProxy = Structural.Proxy.ProtectionProxy.Api.PatternDemo.Create();
        var smartProxy = Structural.Proxy.SmartProxy.Api.PatternDemo.Create();

        Assert.Multiple(() =>
        {
            Assert.That(GetProp(virtualProxy, "Variant"), Is.EqualTo("Virtual Proxy"));
            Assert.That(GetProp(virtualProxy, "RealObjectCreatedCount"), Is.EqualTo(1));

            Assert.That(GetProp(remoteProxy, "Variant"), Is.EqualTo("Remote Proxy"));
            Assert.That(GetProp(remoteProxy, "Forecast"), Is.EqualTo("[https://weather.example/v1] Chennai: 30C, Clear"));

            Assert.That(GetProp(protectionProxy, "Variant"), Is.EqualTo("Protection Proxy"));
            Assert.That(GetProp(protectionProxy, "Admin"), Is.EqualTo("Quarterly margin: 27.3%"));
            Assert.That(GetProp(protectionProxy, "Guest"), Is.EqualTo("Access denied"));

            Assert.That(GetProp(smartProxy, "Variant"), Is.EqualTo("Smart Proxy"));
            Assert.That(GetProp(smartProxy, "BackendCalls"), Is.EqualTo(2));
            Assert.That(GetProp(smartProxy, "CacheHits"), Is.EqualTo(1));
        });
    }

    private static object? GetProp(object target, string name) =>
        target.GetType().GetProperty(name)?.GetValue(target);
}
