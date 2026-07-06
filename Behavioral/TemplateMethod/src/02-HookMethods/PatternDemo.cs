namespace Behavioral.TemplateMethod.HookMethods.Api;

public abstract class Pipeline
{
    public string Run()
    {
        var steps = new List<string>();

        Start(steps);
        BeforeExecute(steps);
        Execute(steps);
        AfterExecute(steps);
        Finish(steps);

        return string.Join(" -> ", steps);
    }

    protected abstract void Start(IList<string> steps);
    protected abstract void Execute(IList<string> steps);
    protected abstract void Finish(IList<string> steps);

    protected virtual void BeforeExecute(IList<string> steps)
    {
    }

    protected virtual void AfterExecute(IList<string> steps)
    {
    }
}

public sealed class AuditPipeline : Pipeline
{
    protected override void Start(IList<string> steps) => steps.Add("Start");
    protected override void Execute(IList<string> steps) => steps.Add("Execute");
    protected override void Finish(IList<string> steps) => steps.Add("Finish");
    protected override void AfterExecute(IList<string> steps) => steps.Add("Audit");
}

public static class HookMethodsDemo
{
    public static object Create()
    {
        Pipeline pipeline = new AuditPipeline();

        return new
        {
            Pattern = "Template Method",
            Variant = "Hook Methods",
            Output = pipeline.Run()
        };
    }
}
