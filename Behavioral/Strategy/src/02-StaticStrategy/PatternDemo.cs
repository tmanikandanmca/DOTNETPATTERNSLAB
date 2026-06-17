namespace Behavioral.Strategy.StaticStrategy.Api;

public interface IStaticTaxStrategy<TSelf> where TSelf : IStaticTaxStrategy<TSelf>
{
    static abstract string Name { get; }
    static abstract decimal TaxFor(decimal amount);
}

public sealed class IndianGstStrategy : IStaticTaxStrategy<IndianGstStrategy>
{
    public static string Name => "India GST (18%)";
    public static decimal TaxFor(decimal amount) => amount * 0.18m;
}

public sealed class UsSalesTaxStrategy : IStaticTaxStrategy<UsSalesTaxStrategy>
{
    public static string Name => "US Sales Tax (7%)";
    public static decimal TaxFor(decimal amount) => amount * 0.07m;
}

public sealed class StaticPricingContext<TStrategy> where TStrategy : IStaticTaxStrategy<TStrategy>
{
    public string StrategyName => TStrategy.Name;

    public decimal TotalWithTax(decimal amount) => amount + TStrategy.TaxFor(amount);
}

public static class StaticStrategyDemo
{
    public static object Create()
    {
        var baseAmount = 100m;
        var india = new StaticPricingContext<IndianGstStrategy>();
        var us = new StaticPricingContext<UsSalesTaxStrategy>();

        return new
        {
            Pattern = "Strategy",
            Variant = "Static Strategy",
            BaseAmount = baseAmount,
            India = new
            {
                india.StrategyName,
                Total = india.TotalWithTax(baseAmount)
            },
            US = new
            {
                us.StrategyName,
                Total = us.TotalWithTax(baseAmount)
            }
        };
    }
}
