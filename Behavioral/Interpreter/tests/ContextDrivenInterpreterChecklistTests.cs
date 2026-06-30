using Behavioral.Interpreter.ContextDrivenInterpreter.Api;

namespace Behavioral.Interpreter.Tests;

public class ContextDrivenInterpreterChecklistTests
{
    [Test]
    public void EachExpression_ShouldBeTestableIndependently()
    {
        var interpreter = RuleInterpreter.CreateDefault();
        var context = BuildContext(baseValue: 10, bonus: 5);

        Assert.Multiple(() =>
        {
            Assert.That(interpreter.Evaluate("standard", context), Is.EqualTo(10));
            Assert.That(interpreter.Evaluate("priority", context), Is.EqualTo(15));
            Assert.That(interpreter.Evaluate("urgent", context), Is.EqualTo(30));
        });
    }

    [Test]
    public void ComposedExpressionTrees_ShouldProduceDeterministicOutput()
    {
        var interpreter = RuleInterpreter.CreateDefault();
        var context = BuildContext(baseValue: 8, bonus: 2);

        var first = interpreter.Evaluate("urgent", context);
        var second = interpreter.Evaluate("urgent", context);

        Assert.That(first, Is.EqualTo(second));
        Assert.That(first, Is.EqualTo(20));
    }

    [Test]
    public void ParserOutput_ShouldMapCorrectlyToExpressionObjects()
    {
        var interpreter = RuleInterpreter.CreateDefault();
        var context = BuildContext(baseValue: 11, bonus: 4);

        Assert.Multiple(() =>
        {
            Assert.That(interpreter.Evaluate("standard", context), Is.EqualTo(11));
            Assert.That(interpreter.Evaluate("priority", context), Is.EqualTo(15));
            Assert.That(interpreter.Evaluate("urgent", context), Is.EqualTo(30));
        });
    }

    [Test]
    public void Context_ShouldRemainExplicit_AndFreeFromHiddenGlobalState()
    {
        var interpreter = RuleInterpreter.CreateDefault();

        var first = BuildContext(baseValue: 3, bonus: 1);
        var second = BuildContext(baseValue: 20, bonus: 2);

        Assert.Multiple(() =>
        {
            Assert.That(interpreter.Evaluate("priority", first), Is.EqualTo(4));
            Assert.That(interpreter.Evaluate("priority", second), Is.EqualTo(22));
        });
    }

    [Test]
    public void AddingANewOperator_ShouldNotRequireRewritingExistingNodes()
    {
        var interpreter = RuleInterpreter.CreateDefault();
        interpreter.Register("critical", context => interpreter.Evaluate("urgent", context) + 5);

        var context = BuildContext(baseValue: 10, bonus: 5);

        Assert.That(interpreter.Evaluate("critical", context), Is.EqualTo(35));
    }

    private static RuleContext BuildContext(int baseValue, int bonus)
    {
        var context = new RuleContext();
        context.Values["base"] = baseValue;
        context.Values["bonus"] = bonus;
        return context;
    }
}
