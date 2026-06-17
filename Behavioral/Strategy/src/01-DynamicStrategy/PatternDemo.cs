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

public sealed class FlatDiscountStrategy(decimal discount) : IPriceStrategy
{
    public string Name => $"Flat {discount:C0} discount";

    public decimal Calculate(decimal amount) => Math.Max(0m, amount - discount);
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
        var percentageContext = new PricingContext(new PercentageDiscountStrategy(0.15m));
        var flatContext = new PricingContext(new FlatDiscountStrategy(20m));
        var amount = 200m;

        return new
        {
            Pattern = "Strategy",
            Dynamic = new
            {
                percentageContext.StrategyName,
                Quote = percentageContext.Quote(amount),
                Alternate = new
                {
                    flatContext.StrategyName,
                    Quote = flatContext.Quote(amount)
                },
                Amount = amount,
                Swappable = true
            }
        };
    }
}
