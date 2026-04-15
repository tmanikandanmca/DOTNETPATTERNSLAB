#:sdk Microsoft.NET.Sdk.Web

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () =>
{
    var request = new
    {
        Endpoint = "/orders",
        Method = "POST",
    };

    return new
    {
        Pattern = "Builder",
        Variant = "Fluent Builder",
        request.Endpoint,
        request.Method
    };
});

app.MapGet("/explain", () => new
{
    pattern = "Builder",
    variant = "Fluent Builder",
    summary = "Builds an object through a readable, chainable fluent API."
});

app.Run();

public partial class Program;
