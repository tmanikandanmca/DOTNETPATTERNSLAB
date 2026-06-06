namespace Behavioral.Observer.PullModel.Api;

public sealed class TemperatureSensor
{
    private readonly List<PullObserver> observers = [];

    public int TemperatureCelsius { get; private set; }

    public void Attach(PullObserver observer) => observers.Add(observer);

    public IReadOnlyList<string> SetTemperature(int temperatureCelsius)
    {
        TemperatureCelsius = temperatureCelsius;
        return observers.Select(observer => observer.Refresh(this)).ToArray();
    }
}

public sealed class PullObserver(string name)
{
    public string Name { get; } = name;

    public string Refresh(TemperatureSensor sensor) => $"{Name} pulled {sensor.TemperatureCelsius} C";
}

public static class PullModelDemo
{
    public static object Create()
    {
        var sensor = new TemperatureSensor();
        sensor.Attach(new PullObserver("Dashboard"));
        sensor.Attach(new PullObserver("Alerting Service"));

        return new
        {
            Pattern = "Observer",
            Variant = "Pull Model",
            Temperature = sensor.SetTemperature(28)
        };
    }
}
