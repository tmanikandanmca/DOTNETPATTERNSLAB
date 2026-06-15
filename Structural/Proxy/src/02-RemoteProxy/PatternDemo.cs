namespace Structural.Proxy.RemoteProxy.Api;

public interface IWeatherService
{
    string Forecast(string city);
}

public interface IWeatherTransport
{
    string FetchForecast(string city);
}

public sealed class RemoteWeatherService : IWeatherService
{
    public string Forecast(string city) => $"{city}: 30C, Clear";
}

public sealed class RemoteWeatherProxy : IWeatherService
{
    private readonly IWeatherTransport _transport;
    private readonly int _maxRetries;

    public RemoteWeatherProxy(IWeatherTransport transport, string endpoint, int maxRetries = 0)
    {
        _transport = transport;
        Endpoint = endpoint;
        _maxRetries = maxRetries;
    }

    public string Endpoint { get; }

    public int Attempts { get; private set; }

    public string Forecast(string city)
    {
        for (var attempt = 0; ; attempt++)
        {
            Attempts++;

            try
            {
                return $"[{Endpoint}] {_transport.FetchForecast(city)}";
            }
            catch when (attempt < _maxRetries)
            {
            }
        }
    }
}

public static class PatternDemo
{
    public static object Create()
    {
        var proxy = new RemoteWeatherProxy(new WeatherTransport(new RemoteWeatherService()), "https://weather.example/v1");

        return new
        {
            Pattern = "Proxy",
            Variant = "Remote Proxy",
            Forecast = proxy.Forecast("Chennai")
        };
    }
}

internal sealed class WeatherTransport : IWeatherTransport
{
    private readonly IWeatherService _remote;

    public WeatherTransport(IWeatherService remote)
    {
        _remote = remote;
    }

    public string FetchForecast(string city) => _remote.Forecast(city);
}
