using Behavioral.ChainOfResponsibility.PureChain.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => ChainDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Chain of Responsibility",
    variant = "Pure Chain",
    summary = "A single handler owns the request end-to-end without forwarding it onward."
});

app.Run();

public partial class Program;
