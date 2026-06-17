using Structural.Proxy.RemoteProxy.Api;

namespace Structural.Proxy.Tests;

public class RemoteProxyChecklistTests
{
    [Test]
    public void ClientCode_WorksWithSubjectInterfaceOnly()
    {
        IWeatherService service = new RemoteWeatherProxy(new RecordingTransport(), "https://weather.example/v1");

        var forecast = service.Forecast("Chennai");

        Assert.That(forecast, Is.EqualTo("[https://weather.example/v1] Chennai: 30C, Clear"));
    }

    [Test]
    public void ProxyControlsTransport_NotBusinessLogicDuplication()
    {
        var transport = new RecordingTransport();
        IWeatherService service = new RemoteWeatherProxy(transport, "https://weather.example/v1");

        var forecast = service.Forecast("Mumbai");

        Assert.Multiple(() =>
        {
            Assert.That(transport.RequestedCities, Is.EqualTo(new[] { "Mumbai" }));
            Assert.That(forecast, Is.EqualTo("[https://weather.example/v1] Mumbai: 30C, Clear"));
        });
    }

    [Test]
    public void TransportFailures_AreReportedSeparately_FromBusinessBehavior()
    {
        var transport = new FailingTransport(failuresBeforeSuccess: int.MaxValue);
        var proxy = new RemoteWeatherProxy(transport, "https://weather.example/v1");

        var error = Assert.Throws<InvalidOperationException>(() => proxy.Forecast("Delhi"));

        Assert.Multiple(() =>
        {
            Assert.That(error!.Message, Is.EqualTo("Transport unavailable."));
            Assert.That(transport.Attempts, Is.EqualTo(1));
        });
    }

    [Test]
    public void RetryPolicy_ReplaysTransportLayer_WithoutChangingBusinessOutput()
    {
        var transport = new FailingTransport(failuresBeforeSuccess: 1);
        var proxy = new RemoteWeatherProxy(transport, "https://weather.example/v1", maxRetries: 1);

        var forecast = proxy.Forecast("Pune");

        Assert.Multiple(() =>
        {
            Assert.That(forecast, Is.EqualTo("[https://weather.example/v1] Pune: 30C, Clear"));
            Assert.That(transport.Attempts, Is.EqualTo(2));
            Assert.That(proxy.Attempts, Is.EqualTo(2));
        });
    }

    private sealed class RecordingTransport : IWeatherTransport
    {
        public List<string> RequestedCities { get; } = new();

        public string FetchForecast(string city)
        {
            RequestedCities.Add(city);
            return $"{city}: 30C, Clear";
        }
    }

    private sealed class FailingTransport : IWeatherTransport
    {
        private readonly int _failuresBeforeSuccess;

        public FailingTransport(int failuresBeforeSuccess)
        {
            _failuresBeforeSuccess = failuresBeforeSuccess;
        }

        public int Attempts { get; private set; }

        public string FetchForecast(string city)
        {
            Attempts++;

            if (Attempts <= _failuresBeforeSuccess)
            {
                throw new InvalidOperationException("Transport unavailable.");
            }

            return $"{city}: 30C, Clear";
        }
    }
}