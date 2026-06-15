using Behavioral.Strategy.StaticStrategy.Api;

namespace Behavioral.Strategy.Tests;

public class StaticStrategyChecklistTests
{
    [Test]
    public void AllConcreteStrategies_ImplementStaticStrategyContract()
    {
        Assert.Multiple(() =>
        {
            Assert.That(HasStaticStrategyInterface(typeof(IndianGstStrategy)), Is.True);
            Assert.That(HasStaticStrategyInterface(typeof(UsSalesTaxStrategy)), Is.True);
        });
    }

    [Test]
    public void Context_DelegatesComputation_ToCompileTimeBoundStrategy()
    {
        var india = new StaticPricingContext<IndianGstStrategy>();
        var us = new StaticPricingContext<UsSalesTaxStrategy>();

        var amount = 100m;

        Assert.Multiple(() =>
        {
            Assert.That(india.TotalWithTax(amount), Is.EqualTo(118m));
            Assert.That(us.TotalWithTax(amount), Is.EqualTo(107m));
        });
    }

    [Test]
    public void Strategies_AreSwappable_ByChangingGenericType_Only()
    {
        var amount = 200m;
        var indiaResult = new StaticPricingContext<IndianGstStrategy>().TotalWithTax(amount);
        var usResult = new StaticPricingContext<UsSalesTaxStrategy>().TotalWithTax(amount);

        Assert.Multiple(() =>
        {
            Assert.That(indiaResult, Is.EqualTo(236m));
            Assert.That(usResult, Is.EqualTo(214m));
        });
    }

    [Test]
    public void NewStaticStrategies_CanBeAdded_WithoutModifyingContext()
    {
        var context = new StaticPricingContext<ZeroTaxStrategy>();

        var total = context.TotalWithTax(150m);

        Assert.That(total, Is.EqualTo(150m));
    }

    [Test]
    public void UnitTests_CanUseDifferentStaticStrategies_ToVerifyContextBehavior()
    {
        var amount = 50m;

        var india = new StaticPricingContext<IndianGstStrategy>().TotalWithTax(amount);
        var us = new StaticPricingContext<UsSalesTaxStrategy>().TotalWithTax(amount);
        var zero = new StaticPricingContext<ZeroTaxStrategy>().TotalWithTax(amount);

        Assert.Multiple(() =>
        {
            Assert.That(india, Is.EqualTo(59m));
            Assert.That(us, Is.EqualTo(53.5m));
            Assert.That(zero, Is.EqualTo(50m));
        });
    }

    private static bool HasStaticStrategyInterface(Type strategyType)
    {
        return strategyType.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStaticTaxStrategy<>));
    }

    private sealed class ZeroTaxStrategy : IStaticTaxStrategy<ZeroTaxStrategy>
    {
        public static string Name => "Zero Tax";

        public static decimal TaxFor(decimal amount) => 0m;
    }
}
