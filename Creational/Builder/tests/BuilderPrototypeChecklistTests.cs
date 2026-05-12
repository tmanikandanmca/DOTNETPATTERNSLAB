using Builder.FluentBuilder.Api;
using Builder.StepBuilder.Api;
using Prototype.CloneRegistry.Api;
using Prototype.DeepCopy.Api;

namespace Builder.UnitTests;

public class BuilderPrototypeChecklistTests
{
    [Test]
    public void DeepPrototypeClone_IsNewReference_NotOriginalObject()
    {
        var prototype = new CustomerProfile
        {
            Name = "Ravi",
            Address = new Address { City = "Madurai" }
        };

        var clone = prototype.DeepClone();

        Assert.Multiple(() =>
        {
            Assert.That(clone, Is.Not.SameAs(prototype));
            Assert.That(clone.Address, Is.Not.SameAs(prototype.Address));
            Assert.That(clone.Name, Is.EqualTo(prototype.Name));
            Assert.That(clone.Address.City, Is.EqualTo(prototype.Address.City));
        });
    }

    [Test]
    public void BuilderAndPrototypeFlow_ProducesIndependentValidOutputs()
    {
        var registry = new DocumentRegistry();
        registry.Register("invoice", new TemplateDocument { Title = "Invoice Template", Category = "Billing" });

        var firstPrototypeClone = registry.Create("invoice");
        var secondPrototypeClone = registry.Create("invoice");

        var request = new ApiRequestBuilder()
            .WithEndpoint($"/docs/{firstPrototypeClone.Title.Replace(" ", "-").ToLowerInvariant()}")
            .UsingMethod("POST")
            .AddHeader("x-category", firstPrototypeClone.Category)
            .Build();

        var plan = DeploymentPlanBuilder.Create()
            .Named(secondPrototypeClone.Title)
            .ForEnvironment("Production")
            .InRegion("eu-west")
            .WithMonitoring()
            .Build();

        firstPrototypeClone.Title = "Mutated Clone";
        var freshClone = registry.Create("invoice");

        Assert.Multiple(() =>
        {
            Assert.That(firstPrototypeClone, Is.Not.SameAs(secondPrototypeClone));
            Assert.That(request.Endpoint, Does.Contain("invoice-template"));
            Assert.That(request.Headers["x-category"], Is.EqualTo("Billing"));
            Assert.That(plan.Name, Is.EqualTo("Invoice Template"));
            Assert.That(plan.MonitoringEnabled, Is.True);
            Assert.That(freshClone.Title, Is.EqualTo("Invoice Template"));
        });
    }
}
