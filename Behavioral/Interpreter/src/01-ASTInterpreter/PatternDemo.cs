namespace Behavioral.Interpreter.ASTInterpreter.Api;

public interface IExpression
{
    int Interpret(IReadOnlyDictionary<string, int> context);
}

public sealed class NumberExpression(int value) : IExpression
{
    public int Interpret(IReadOnlyDictionary<string, int> context) => value;
}

public sealed class VariableExpression(string name) : IExpression
{
    public int Interpret(IReadOnlyDictionary<string, int> context) => context[name];
}

public sealed class AddExpression(IExpression left, IExpression right) : IExpression
{
    public int Interpret(IReadOnlyDictionary<string, int> context) => left.Interpret(context) + right.Interpret(context);
}

public sealed class MultiplyExpression(IExpression left, IExpression right) : IExpression
{
    public int Interpret(IReadOnlyDictionary<string, int> context) => left.Interpret(context) * right.Interpret(context);
}

public static class PrefixExpressionParser
{
    public static IExpression Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Expression cannot be empty.", nameof(input));
        }

        var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var index = 0;
        var expression = ParseNode(tokens, ref index);

        if (index != tokens.Length)
        {
            throw new ArgumentException("Unexpected trailing tokens.", nameof(input));
        }

        return expression;
    }

    private static IExpression ParseNode(string[] tokens, ref int index)
    {
        if (index >= tokens.Length)
        {
            throw new ArgumentException("Unexpected end of expression.");
        }

        var token = tokens[index++];

        if (int.TryParse(token, out var constant))
        {
            return new NumberExpression(constant);
        }

        return token switch
        {
            "add" => new AddExpression(ParseNode(tokens, ref index), ParseNode(tokens, ref index)),
            "mul" => new MultiplyExpression(ParseNode(tokens, ref index), ParseNode(tokens, ref index)),
            _ => new VariableExpression(token)
        };
    }
}

public static class InterpreterDemo
{
    public static object Create()
    {
        var expression = PrefixExpressionParser.Parse("add 2 mul x 3");

        IReadOnlyDictionary<string, int> context = new Dictionary<string, int> { ["x"] = 4 };

        return new
        {
            Pattern = "Interpreter",
            Variant = "AST Interpreter",
            AstResult = expression.Interpret(context)
        };
    }
}
