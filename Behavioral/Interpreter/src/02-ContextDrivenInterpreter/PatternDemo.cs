namespace Behavioral.Interpreter.ContextDrivenInterpreter.Api;

public sealed class RuleContext
{
    public Dictionary<string, int> Values { get; } = new(StringComparer.OrdinalIgnoreCase);
}

public sealed class RuleInterpreter
{
    private readonly Dictionary<string, Func<RuleContext, int>> _operators;

    public RuleInterpreter()
    {
        _operators = new(StringComparer.OrdinalIgnoreCase);
    }

    public static RuleInterpreter CreateDefault()
    {
        var interpreter = new RuleInterpreter();
        interpreter.Register("standard", context => context.Values["base"]);
        interpreter.Register("priority", context => context.Values["base"] + context.Values["bonus"]);
        interpreter.Register("urgent", context => (context.Values["base"] + context.Values["bonus"]) * 2);
        return interpreter;
    }

    public void Register(string rule, Func<RuleContext, int> evaluator)
    {
        _operators[rule] = evaluator;
    }

    public int Evaluate(string rule, RuleContext context)
    {
        if (_operators.TryGetValue(rule, out var evaluator))
        {
            return evaluator(context);
        }

        return 0;
    }
}

public static class ContextDrivenInterpreterDemo
{
    public static object Create()
    {
        var interpreter = RuleInterpreter.CreateDefault();
        var context = new RuleContext();
        context.Values["base"] = 10;
        context.Values["bonus"] = 5;

        return new
        {
            Pattern = "Interpreter",
            Variant = "Context-driven Interpreter",
            Standard = interpreter.Evaluate("standard", context),
            Priority = interpreter.Evaluate("priority", context),
            Urgent = interpreter.Evaluate("urgent", context)
        };
    }
}
