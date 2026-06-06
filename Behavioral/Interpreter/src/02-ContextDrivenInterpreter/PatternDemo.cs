namespace Behavioral.Interpreter.ContextDrivenInterpreter.Api;

public sealed class RuleContext
{
    public Dictionary<string, int> Values { get; } = new(StringComparer.OrdinalIgnoreCase);
}

public static class RuleInterpreter
{
    public static int Evaluate(string rule, RuleContext context)
        => rule switch
        {
            "standard" => context.Values["base"],
            "priority" => context.Values["base"] + context.Values["bonus"],
            "urgent" => (context.Values["base"] + context.Values["bonus"]) * 2,
            _ => 0
        };
}

public static class ContextDrivenInterpreterDemo
{
    public static object Create()
    {
        var context = new RuleContext();
        context.Values["base"] = 10;
        context.Values["bonus"] = 5;

        return new
        {
            Pattern = "Interpreter",
            Variant = "Context-driven Interpreter",
            Standard = RuleInterpreter.Evaluate("standard", context),
            Priority = RuleInterpreter.Evaluate("priority", context),
            Urgent = RuleInterpreter.Evaluate("urgent", context)
        };
    }
}
