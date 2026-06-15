using Behavioral.Strategy.DynamicStrategy.Api;

namespace Behavioral.Strategy.Tests;

public class DynamicStrategyChecklistTests
{
    [Test]
    public void AllConcreteStrategies_ImplementSameInterface()
    {
        var percentage = new PercentageDiscountStrategy(0.10m);
        var flat = new FlatDiscountStrategy(10m);

        Assert.Multiple(() =>
        {
            Assert.That(percentage, Is.InstanceOf<IPriceStrategy>());
            Assert.That(flat, Is.InstanceOf<IPriceStrategy>());
        });
    }

    [Test]
    public void Context_DelegatesToInjectedStrategy_WithoutHardcodedAlgorithm()
    {
        var probe = new ProbeStrategy(result: 42m);
        var context = new PricingContext(probe);

        var quote = context.Quote(500m);

        Assert.Multiple(() =>
        {
            Assert.That(quote, Is.EqualTo(42m));
            Assert.That(probe.Calls, Is.EqualTo(1));
            Assert.That(probe.LastAmount, Is.EqualTo(500m));
        });
    }

    [Test]
    public void Strategies_AreSwappable_WithoutChangingContextType()
    {
        var amount = 200m;

        var percentageContext = new PricingContext(new PercentageDiscountStrategy(0.10m));
        var flatContext = new PricingContext(new FlatDiscountStrategy(30m));

        Assert.Multiple(() =>
        {
            Assert.That(percentageContext.Quote(amount), Is.EqualTo(180m));
            Assert.That(flatContext.Quote(amount), Is.EqualTo(170m));
        });
    }

    [Test]
    public void NewStrategies_CanBeAdded_WithoutModifyingContext()
    {
        var context = new PricingContext(new MinimumPriceStrategy(minimum: 50m));

        var lowAmount = context.Quote(20m);
        var highAmount = context.Quote(80m);

        Assert.Multiple(() =>
        {
            Assert.That(lowAmount, Is.EqualTo(50m));
            Assert.That(highAmount, Is.EqualTo(80m));
        });
    }

    [Test]
    public void UnitTests_CanInjectDifferentStrategies_ToVerifyContextBehavior()
    {
        var scenarios = new (IPriceStrategy Strategy, decimal Amount, decimal Expected)[]
        {
            (new PercentageDiscountStrategy(0.20m), 100m, 80m),
            (new FlatDiscountStrategy(15m), 100m, 85m),
            (new MinimumPriceStrategy(90m), 80m, 90m)
        };

        foreach (var scenario in scenarios)
        {
            var context = new PricingContext(scenario.Strategy);
            var result = context.Quote(scenario.Amount);
            Assert.That(result, Is.EqualTo(scenario.Expected));
        }
    }

    private sealed class ProbeStrategy(decimal result) : IPriceStrategy
    {
        public int Calls { get; private set; }

        public decimal LastAmount { get; private set; }

        public string Name => "Probe";

        public decimal Calculate(decimal amount)
        {
            Calls++;
            LastAmount = amount;
            return result;
        }
    }

    private sealed class MinimumPriceStrategy(decimal minimum) : IPriceStrategy
    {
        public string Name => "MinimumPrice";

        public decimal Calculate(decimal amount) => Math.Max(minimum, amount);
    }
}
