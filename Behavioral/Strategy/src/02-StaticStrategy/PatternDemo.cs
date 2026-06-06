namespace Behavioral.Strategy.StaticStrategy.Api;

public static class TaxStrategies
{
    public static decimal IndianGst(decimal amount) => amount * 0.18m;
    public static decimal UsSalesTax(decimal amount) => amount * 0.07m;
}

public static class StaticStrategyDemo
{
    public static object Create()
    {
        var baseAmount = 100m;

        return new
        {
            Pattern = "Strategy",
            Variant = "Static Strategy",
            BaseAmount = baseAmount,
            IndiaTotal = baseAmount + TaxStrategies.IndianGst(baseAmount),
            UsTotal = baseAmount + TaxStrategies.UsSalesTax(baseAmount)
        };
    }
}
