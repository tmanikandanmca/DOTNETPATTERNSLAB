namespace AbstractFactory.FactoryOfFactories.Api;

public interface IVehicleFactory
{
    string Tier { get; }
    string CreateCar();
    string CreateBike();
}

public sealed class EconomyVehicleFactory : IVehicleFactory
{
    public string Tier => "Economy";
    public string CreateCar() => "Hatchback";
    public string CreateBike() => "City Bike";
}

public sealed class LuxuryVehicleFactory : IVehicleFactory
{
    public string Tier => "Luxury";
    public string CreateCar() => "Sports Sedan";
    public string CreateBike() => "Premium Tourer";
}

public static class VehicleFactoryProvider
{
    public static IVehicleFactory Create(string tier)
        => tier.Equals("luxury", StringComparison.OrdinalIgnoreCase)
            ? new LuxuryVehicleFactory()
            : new EconomyVehicleFactory();
}

public static class FactoryOfFactoriesDemo
{
    public static object Create()
    {
        var factory = VehicleFactoryProvider.Create("luxury");
        return new
        {
            Pattern = "Abstract Factory",
            Variant = "Factory of Factories",
            Tier = factory.Tier,
            Car = factory.CreateCar(),
            Bike = factory.CreateBike()
        };
    }
}
