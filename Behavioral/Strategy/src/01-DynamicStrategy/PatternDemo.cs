namespace Behavioral.Strategy.DynamicStrategy.Api;

public interface IPriceStrategy
{
    string Name { get; }
    decimal Calculate(decimal amount);
}

public sealed class PercentageDiscountStrategy(decimal percentage) : IPriceStrategy
{
    public string Name => $"{percentage:P0} discount";

    public decimal Calculate(decimal amount) => amount - (amount * percentage);
}

public static class StaticPricingStrategy
{
    public static decimal CalculatePremium(decimal amount) => amount * 1.10m;
}

public sealed class PricingContext(IPriceStrategy strategy)
{
    public string StrategyName => strategy.Name;

    public decimal Quote(decimal amount) => strategy.Calculate(amount);
}

public static class StrategyDemo
{
    public static object Create()
    {
        var dynamicStrategy = new PricingContext(new PercentageDiscountStrategy(0.15m));
        var amount = 200m;

        return new
        {
            Pattern = "Strategy",
            Dynamic = new
            {
                dynamicStrategy.StrategyName,
                Amount = amount,
                Quote = dynamicStrategy.Quote(amount)
            },
            Static = new
            {
                Strategy = "StaticPricingStrategy.CalculatePremium",
                Amount = amount,
                Quote = StaticPricingStrategy.CalculatePremium(amount)
            }
        };
    }
}
