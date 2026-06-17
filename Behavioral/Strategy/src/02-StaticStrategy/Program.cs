using Behavioral.Strategy.StaticStrategy.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => StaticStrategyDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Strategy",
    variant = "Static Strategy",
    summary = "Compile-time selected strategy methods are called directly."
});

app.Run();

public partial class Program;
