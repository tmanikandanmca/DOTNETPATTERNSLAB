using Prototype.CloneRegistry.Api;

namespace Prototype.UnitTests;

public class CloneRegistryServiceTests
{
    [Test]
    public void Create_ReturnsValueEqualClone_ButNotStoredPrototypeReference()
    {
        var prototype = new TemplateDocument { Title = "Invoice Template", Category = "Billing" };
        var registry = new DocumentRegistry();
        registry.Register("invoice", prototype);

        var clone = registry.Create("invoice");

        Assert.Multiple(() =>
        {
            Assert.That(clone, Is.Not.SameAs(prototype));
            Assert.That(clone.Title, Is.EqualTo(prototype.Title));
            Assert.That(clone.Category, Is.EqualTo(prototype.Category));
        });
    }

    [Test]
    public void Create_ReturnsFreshClone_OnEveryCall()
    {
        var registry = new DocumentRegistry();
        registry.Register("invoice", new TemplateDocument { Title = "Invoice Template", Category = "Billing" });

        var first = registry.Create("invoice");
        var second = registry.Create("invoice");

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(first.Title, Is.EqualTo(second.Title));
            Assert.That(first.Category, Is.EqualTo(second.Category));
        });
    }

    [Test]
    public void ChangingReturnedClone_DoesNotAffectFutureClones()
    {
        var registry = new DocumentRegistry();
        registry.Register("invoice", new TemplateDocument { Title = "Invoice Template", Category = "Billing" });

        var first = registry.Create("invoice");
        first.Title = "Mutated Title";

        var second = registry.Create("invoice");

        Assert.Multiple(() =>
        {
            Assert.That(second.Title, Is.EqualTo("Invoice Template"));
            Assert.That(second.Category, Is.EqualTo("Billing"));
        });
    }

    [Test]
    public void Create_ThrowsClearError_ForUnknownKey()
    {
        var registry = new DocumentRegistry();

        var ex = Assert.Throws<KeyNotFoundException>(() => registry.Create("missing"));

        Assert.That(ex!.Message, Does.Contain("missing"));
    }
}
