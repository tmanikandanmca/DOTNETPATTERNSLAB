namespace Behavioral.TemplateMethod.HookMethods.Api;

public abstract class Pipeline
{
    public string Run()
    {
        var result = Start() + " -> " + Execute();
        return BeforeFinish() ? result + " -> " + Finish() : result;
    }

    protected abstract string Start();
    protected abstract string Execute();
    protected abstract string Finish();

    protected virtual bool BeforeFinish() => false;
}

public sealed class AuditPipeline : Pipeline
{
    protected override string Start() => "Start";
    protected override string Execute() => "Execute";
    protected override string Finish() => "Finish";
    protected override bool BeforeFinish() => true;
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
