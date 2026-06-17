using Behavioral.Strategy.DynamicStrategy.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => StrategyDemo.Create());
app.MapGet("/explain", () => new
{
    pattern = "Strategy",
    variant = "Dynamic Strategy",
    summary = "A runtime-injected strategy selects the algorithm without changing the caller."
});

app.Run();

public partial class Program;
