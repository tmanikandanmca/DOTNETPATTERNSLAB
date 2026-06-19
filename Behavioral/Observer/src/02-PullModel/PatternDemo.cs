namespace Behavioral.Observer.PullModel.Api;

public interface IPullObserver
{
    string Name { get; }
    string Refresh(TemperatureSensor sensor);
}

public sealed class TemperatureSensor
{
    private readonly List<IPullObserver> observers = [];

    public int TemperatureCelsius { get; private set; }

    public void Attach(IPullObserver observer) => observers.Add(observer);

    public bool Detach(IPullObserver observer) => observers.Remove(observer);

    public IReadOnlyList<string> SetTemperature(int temperatureCelsius)
    {
        TemperatureCelsius = temperatureCelsius;
        return observers.Select(observer => observer.Refresh(this)).ToArray();
    }
}

public sealed class PullObserver(string name) : IPullObserver
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
