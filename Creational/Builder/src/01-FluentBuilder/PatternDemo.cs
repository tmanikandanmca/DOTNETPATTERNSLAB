namespace Builder.FluentBuilder.Api;

public sealed record ApiRequest(string Endpoint, string Method, IReadOnlyDictionary<string, string> Headers);

public sealed class ApiRequestBuilder
{
    private string _endpoint = "/";
    private string _method = "GET";
    private readonly Dictionary<string, string> _headers = new();

    public ApiRequestBuilder WithEndpoint(string endpoint)
    {
        _endpoint = endpoint;
        return this;
    }

    public ApiRequestBuilder UsingMethod(string method)
    {
        _method = method;
        return this;
    }

    public ApiRequestBuilder AddHeader(string key, string value)
    {
        _headers[key] = value;
        return this;
    }

    public ApiRequest Build() => new(_endpoint, _method, new Dictionary<string, string>(_headers));
}

public static class FluentBuilderDemo
{
    public static object Create()
    {
        var request = new ApiRequestBuilder()
            .WithEndpoint("/orders")
            .UsingMethod("POST")
            .AddHeader("x-correlation-id", Guid.NewGuid().ToString("N"))
            .Build();

        return new
        {
            Pattern = "Builder",
            Variant = "Fluent Builder",
            request.Endpoint,
            request.Method,
            Headers = request.Headers
        };
    }
}
