namespace Structural.Proxy.RemoteProxy.Api;

public interface IWeatherService
{
    string Forecast(string city);
}

public sealed class RemoteWeatherService : IWeatherService
{
    public string Forecast(string city) => $"{city}: 30C, Clear";
}

public sealed class RemoteWeatherProxy : IWeatherService
{
    private readonly IWeatherService _remote;

    public RemoteWeatherProxy(IWeatherService remote, string endpoint)
    {
        _remote = remote;
        Endpoint = endpoint;
    }

    public string Endpoint { get; }

    public string Forecast(string city) => $"[{Endpoint}] {_remote.Forecast(city)}";
}

public static class PatternDemo
{
    public static object Create()
    {
        var proxy = new RemoteWeatherProxy(new RemoteWeatherService(), "https://weather.example/v1");

        return new
        {
            Pattern = "Proxy",
            Variant = "Remote Proxy",
            Forecast = proxy.Forecast("Chennai")
        };
    }
}
