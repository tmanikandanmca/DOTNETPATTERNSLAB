using Builder.StepBuilder.Api;

namespace Builder.UnitTests;

public class StepBuilderChecklistTests
{
    [Test]
    public void StepInterfaces_EnforceOrderedConstructionFlow()
    {
        INameStep start = DeploymentPlanBuilder.Create();
        var afterName = start.Named("Payments");
        var afterEnvironment = afterName.ForEnvironment("Production");
        var optional = afterEnvironment.InRegion("eu-west");

        var plan = optional.WithMonitoring().Build();

        Assert.Multiple(() =>
        {
            Assert.That(start, Is.InstanceOf<INameStep>());
            Assert.That(afterName, Is.InstanceOf<IEnvironmentStep>());
            Assert.That(afterEnvironment, Is.InstanceOf<IRegionStep>());
            Assert.That(optional, Is.InstanceOf<IOptionalStep>());
            Assert.That(plan.Name, Is.EqualTo("Payments"));
            Assert.That(plan.Environment, Is.EqualTo("Production"));
            Assert.That(plan.Region, Is.EqualTo("eu-west"));
            Assert.That(plan.MonitoringEnabled, Is.True);
        });
    }

    [Test]
    public void EachBuild_ProducesValidIndependentDeploymentPlan()
    {
        var first = DeploymentPlanBuilder.Create()
            .Named("Payments")
            .ForEnvironment("Production")
            .InRegion("eu-west")
            .WithMonitoring()
            .Build();

        var second = DeploymentPlanBuilder.Create()
            .Named("Analytics")
            .ForEnvironment("Staging")
            .InRegion("us-east")
            .Build();

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(first.Name, Is.EqualTo("Payments"));
            Assert.That(first.MonitoringEnabled, Is.True);
            Assert.That(second.Name, Is.EqualTo("Analytics"));
            Assert.That(second.MonitoringEnabled, Is.False);
        });
    }
}
