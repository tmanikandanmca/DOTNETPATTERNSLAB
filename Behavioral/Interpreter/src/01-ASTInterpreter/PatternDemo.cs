namespace Behavioral.Interpreter.ASTInterpreter.Api;

public interface IExpression
{
    int Interpret(Dictionary<string, int> context);
}

public sealed class NumberExpression(int value) : IExpression
{
    public int Interpret(Dictionary<string, int> context) => value;
}

public sealed class VariableExpression(string name) : IExpression
{
    public int Interpret(Dictionary<string, int> context) => context[name];
}

public sealed class AddExpression(IExpression left, IExpression right) : IExpression
{
    public int Interpret(Dictionary<string, int> context) => left.Interpret(context) + right.Interpret(context);
}

public sealed class MultiplyExpression(IExpression left, IExpression right) : IExpression
{
    public int Interpret(Dictionary<string, int> context) => left.Interpret(context) * right.Interpret(context);
}

public sealed class RuleContext
{
    public Dictionary<string, int> Values { get; } = new(StringComparer.OrdinalIgnoreCase);

    public int Evaluate(string rule)
        => rule switch
        {
            "highPriority" => Values["priority"] * 2,
            "normalPriority" => Values["priority"],
            _ => 0
        };
}

public static class InterpreterDemo
{
    public static object Create()
    {
        IExpression expression = new AddExpression(
            new NumberExpression(2),
            new MultiplyExpression(new VariableExpression("x"), new NumberExpression(3)));

        var context = new Dictionary<string, int> { ["x"] = 4 };
        var ruleContext = new RuleContext();
        ruleContext.Values["priority"] = 5;

        return new
        {
            Pattern = "Interpreter",
            AstResult = expression.Interpret(context),
            ContextDrivenResult = ruleContext.Evaluate("highPriority")
        };
    }
}
