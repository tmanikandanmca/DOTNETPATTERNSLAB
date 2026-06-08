using Structural.Proxy.ProtectionProxy.Api;

namespace Structural.Proxy.Tests;

public class ProtectionProxyChecklistTests
{
    [Test]
    public void ClientCode_WorksWithSubjectInterfaceOnly()
    {
        IConfidentialReport report = new ProtectionProxyReport(new ConfidentialReport(), "admin");

        var result = report.Read();

        Assert.That(result, Is.EqualTo("Quarterly margin: 27.3%"));
    }

    [Test]
    public void ProxyAllowsAuthorizedAccess()
    {
        IConfidentialReport report = new ProtectionProxyReport(new ConfidentialReport(), "admin");

        var result = report.Read();

        Assert.That(result, Is.EqualTo("Quarterly margin: 27.3%"));
    }

    [Test]
    public void ProxyDeniesUnauthorizedAccess()
    {
        IConfidentialReport report = new ProtectionProxyReport(new ConfidentialReport(), "guest");

        var result = report.Read();

        Assert.That(result, Is.EqualTo("Access denied"));
    }

    [Test]
    public void ProxyControlsAccess_RatherThanDuplicatingBusinessLogic()
    {
        var inner = new RecordingReport();
        IConfidentialReport report = new ProtectionProxyReport(inner, "guest");

        var denied = report.Read();

        Assert.Multiple(() =>
        {
            Assert.That(denied, Is.EqualTo("Access denied"));
            Assert.That(inner.ReadCalls, Is.EqualTo(0));
        });
    }

    private sealed class RecordingReport : IConfidentialReport
    {
        public int ReadCalls { get; private set; }

        public string Read()
        {
            ReadCalls++;
            return "Quarterly margin: 27.3%";
        }
    }
}