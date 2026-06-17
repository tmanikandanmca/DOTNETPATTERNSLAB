using Structural.Proxy.VirtualProxy.Api;

namespace Structural.Proxy.Tests;

public class VirtualProxyChecklistTests
{
    [Test]
    public void ClientCode_WorksWithSubjectInterfaceOnly()
    {
        IDocument document = new VirtualDocumentProxy();

        var firstRead = document.Read();

        Assert.That(firstRead, Does.StartWith("Loaded at "));
    }

    [Test]
    public void RealObject_IsNotCreatedUntilFirstCall()
    {
        HeavyDocument.CreatedCount = 0;

        IDocument document = new VirtualDocumentProxy();

        Assert.That(HeavyDocument.CreatedCount, Is.EqualTo(0));

        _ = document.Read();

        Assert.That(HeavyDocument.CreatedCount, Is.EqualTo(1));
    }

    [Test]
    public void RealObject_IsReused_AfterInitialization()
    {
        HeavyDocument.CreatedCount = 0;

        IDocument document = new VirtualDocumentProxy();

        var first = document.Read();
        var second = document.Read();

        Assert.Multiple(() =>
        {
            Assert.That(HeavyDocument.CreatedCount, Is.EqualTo(1));
            Assert.That(second, Is.EqualTo(first));
        });
    }

    [Test]
    public void ProxyControlsLazyInitialization_NotBusinessLogicDuplication()
    {
        HeavyDocument.CreatedCount = 0;

        IDocument document = new VirtualDocumentProxy();

        var result = document.Read();

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.StartWith("Loaded at "));
            Assert.That(HeavyDocument.CreatedCount, Is.EqualTo(1));
        });
    }
}