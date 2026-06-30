using Behavioral.Interpreter.ASTInterpreter.Api;

namespace Behavioral.Interpreter.Tests;

public class ASTInterpreterChecklistTests
{
    [Test]
    public void EachExpression_ShouldBeTestableIndependently()
    {
        IReadOnlyDictionary<string, int> context = new Dictionary<string, int> { ["x"] = 4 };

        var number = new NumberExpression(5);
        var variable = new VariableExpression("x");
        var add = new AddExpression(new NumberExpression(2), new NumberExpression(3));
        var multiply = new MultiplyExpression(new NumberExpression(3), new NumberExpression(4));

        Assert.Multiple(() =>
        {
            Assert.That(number.Interpret(context), Is.EqualTo(5));
            Assert.That(variable.Interpret(context), Is.EqualTo(4));
            Assert.That(add.Interpret(context), Is.EqualTo(5));
            Assert.That(multiply.Interpret(context), Is.EqualTo(12));
        });
    }

    [Test]
    public void ComposedExpressionTrees_ShouldProduceDeterministicOutput()
    {
        IExpression expression = new AddExpression(
            new NumberExpression(2),
            new MultiplyExpression(new VariableExpression("x"), new NumberExpression(3)));

        IReadOnlyDictionary<string, int> context = new Dictionary<string, int> { ["x"] = 4 };

        var first = expression.Interpret(context);
        var second = expression.Interpret(context);

        Assert.That(first, Is.EqualTo(second));
        Assert.That(first, Is.EqualTo(14));
    }

    [Test]
    public void ParserOutput_ShouldMapCorrectlyToExpressionObjects()
    {
        var expression = PrefixExpressionParser.Parse("add 2 mul x 3");

        Assert.That(expression, Is.TypeOf<AddExpression>());

        var context = new Dictionary<string, int> { ["x"] = 5 };
        Assert.That(expression.Interpret(context), Is.EqualTo(17));
    }

    [Test]
    public void Context_ShouldRemainExplicit_AndFreeFromHiddenGlobalState()
    {
        var expression = PrefixExpressionParser.Parse("add x 1");

        IReadOnlyDictionary<string, int> left = new Dictionary<string, int> { ["x"] = 1 };
        IReadOnlyDictionary<string, int> right = new Dictionary<string, int> { ["x"] = 9 };

        Assert.Multiple(() =>
        {
            Assert.That(expression.Interpret(left), Is.EqualTo(2));
            Assert.That(expression.Interpret(right), Is.EqualTo(10));
        });
    }

    [Test]
    public void AddingANewOperator_ShouldNotRequireRewritingExistingNodes()
    {
        var expression = new SquareExpression(new AddExpression(new NumberExpression(2), new NumberExpression(3)));

        var result = expression.Interpret(new Dictionary<string, int>());

        Assert.That(result, Is.EqualTo(25));
    }

    private sealed class SquareExpression(IExpression inner) : IExpression
    {
        public int Interpret(IReadOnlyDictionary<string, int> context)
        {
            var value = inner.Interpret(context);
            return value * value;
        }
    }
}
