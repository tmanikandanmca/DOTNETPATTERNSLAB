namespace Builder.StepBuilder.Api;

public sealed record DeploymentPlan(string Name, string Environment, string Region, bool MonitoringEnabled);

public interface INameStep { IEnvironmentStep Named(string name); }
public interface IEnvironmentStep { IRegionStep ForEnvironment(string environment); }
public interface IRegionStep { IOptionalStep InRegion(string region); }
public interface IOptionalStep
{
    IOptionalStep WithMonitoring(bool enabled = true);
    DeploymentPlan Build();
}

public sealed class DeploymentPlanBuilder : INameStep, IEnvironmentStep, IRegionStep, IOptionalStep
{
    private string _name = string.Empty;
    private string _environment = string.Empty;
    private string _region = string.Empty;
    private bool _monitoringEnabled;

    public static INameStep Create() => new DeploymentPlanBuilder();

    public IEnvironmentStep Named(string name)
    {
        _name = name;
        return this;
    }

    public IRegionStep ForEnvironment(string environment)
    {
        _environment = environment;
        return this;
    }

    public IOptionalStep InRegion(string region)
    {
        _region = region;
        return this;
    }

    public IOptionalStep WithMonitoring(bool enabled = true)
    {
        _monitoringEnabled = enabled;
        return this;
    }

    public DeploymentPlan Build() => new(_name, _environment, _region, _monitoringEnabled);
}

public static class StepBuilderDemo
{
    public static object Create()
    {
        var plan = DeploymentPlanBuilder.Create()
            .Named("Payments")
            .ForEnvironment("Production")
            .InRegion("eu-west")
            .WithMonitoring()
            .Build();

        return new
        {
            Pattern = "Builder",
            Variant = "Step Builder",
            plan.Name,
            plan.Environment,
            plan.Region,
            plan.MonitoringEnabled
        };
    }
}
