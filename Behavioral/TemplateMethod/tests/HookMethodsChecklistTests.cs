using Behavioral.TemplateMethod.HookMethods.Api;

namespace Behavioral.TemplateMethod.UnitTests;

public class HookMethodsChecklistTests
{
    [Test]
    public void BaseClass_OwnsTheSequence()
    {
        var pipeline = new RecordingPipeline();

        _ = pipeline.Run();

        Assert.That(
            pipeline.Steps,
            Is.EqualTo(new[] { "Start", "BeforeExecute", "Execute", "AfterExecute", "Finish" }));
    }

    [Test]
    public void HookMethods_AreOptional_AndSafeByDefault()
    {
        Pipeline pipeline = new MinimalPipeline();

        var result = pipeline.Run();

        Assert.That(result, Is.EqualTo("Start -> Execute -> Finish"));
    }

    [Test]
    public void Tests_ConfirmTheOrder_DoesNotChangeAccidentally()
    {
        Pipeline pipeline = new AuditPipeline();

        var result = pipeline.Run();

        Assert.That(result, Is.EqualTo("Start -> Execute -> Audit -> Finish"));
    }

    [Test]
    public void DerivedClasses_ShouldNotDuplicateTheFullAlgorithm()
    {
        var runOverride = typeof(RecordingPipeline).GetMethod(
            nameof(Pipeline.Run),
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.DeclaredOnly);

        Assert.That(runOverride, Is.Null);
    }

    private sealed class MinimalPipeline : Pipeline
    {
        protected override void Start(IList<string> steps) => steps.Add("Start");

        protected override void Execute(IList<string> steps) => steps.Add("Execute");

        protected override void Finish(IList<string> steps) => steps.Add("Finish");
    }

    private sealed class RecordingPipeline : Pipeline
    {
        public List<string> Steps { get; } = new();

        protected override void Start(IList<string> steps)
        {
            Steps.Add("Start");
            steps.Add("Start");
        }

        protected override void BeforeExecute(IList<string> steps)
        {
            Steps.Add("BeforeExecute");
            steps.Add("BeforeExecute");
        }

        protected override void Execute(IList<string> steps)
        {
            Steps.Add("Execute");
            steps.Add("Execute");
        }

        protected override void AfterExecute(IList<string> steps)
        {
            Steps.Add("AfterExecute");
            steps.Add("AfterExecute");
        }

        protected override void Finish(IList<string> steps)
        {
            Steps.Add("Finish");
            steps.Add("Finish");
        }
    }
}